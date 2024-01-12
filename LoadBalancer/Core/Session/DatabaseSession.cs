using LoadBalancer.Abstracts;
using NHibernate;
using NHibernate.Cfg;

namespace LoadBalancer.Core.Session;

public class DatabaseSession : ManageableSession, IUnitOfWork
{
    private LoadBalancerInterceptor interceptor;
    private LinkedList<DbRequest> queue;
    private ISession session;
    private ISession interceptedSession;
    private string configFileName;
    
    public DatabaseSession(LoadBalancerInterceptor interceptor, string configFileName)
    {
        this.configFileName = configFileName;
        this.interceptor = interceptor;
        Console.WriteLine($"[NHIBERNATE SESSION] Creating session with file name: {configFileName}");
        try
        {
            Connect();
        }
        catch (Exception e)
        {
            this.status = Status.DOWN;
            Console.WriteLine($"[NHIBERNATE SESSION '{this.configFileName}'] Could not create connection. Details: {e.Message}");
            Console.WriteLine(e);
        }
        this.queue = new LinkedList<DbRequest>();
    }
    
    private void Connect()
    {
        var configuration = new Configuration();
        configuration.Configure(this.configFileName);

        var schemaExport = new NHibernate.Tool.hbm2ddl.SchemaExport(configuration);
        schemaExport.Create(true, true);

        var sessionFactory = configuration.BuildSessionFactory();
        session = sessionFactory.OpenSession();
        interceptedSession = sessionFactory.WithOptions().Interceptor(interceptor).OpenSession();
        status = Status.UP;
        Console.WriteLine($"[NHIBERNATE SESSION '{configFileName}'] Connection established");
    }

    private void Close()
    {
        syncChanges();
        
        session.Close();
        session.SessionFactory.Close();
        interceptedSession.Close();
        interceptedSession.SessionFactory.Close();
        this.status = Status.DOWN;
    }

    public override void execute(DbRequest request)
    {
        try
        {
            switch (request.getType())
            {
                case DbRequest.Type.INSERT:
                    session.Save(request.getObject());
                    break;
                case DbRequest.Type.UPDATE:
                    session.Update(request.getObject());
                    break;
                case DbRequest.Type.DELETE:
                    session.Delete(request.getObject());
                    break;
                default:
                    throw new NotSupportedException($"Operation '{request.getType()}' is not supported");
            }
        }
        catch (NotSupportedException exception)
        {
            register(request);
            Console.WriteLine($"[NHIBERNATE SESSION '{configFileName}'] Could not execute request. Details: {exception.Message}");
            throw;
        }
    }

    public void executeSingleRequest(DbRequest request)
    {
        try
        {
            if (!session.GetCurrentTransaction().IsActive)
                session.BeginTransaction();

            execute(request);

            session.GetCurrentTransaction().Commit();
            session.Clear();
        }
        catch (Exception e)
        {
            Console.WriteLine(
                $"[NHIBERNATE SESSION '{configFileName}'] Could not execute request. Details: {e.Message}");
            Console.WriteLine(e);
            session.GetCurrentTransaction().Rollback();
            session.Clear();
        }
    }

    public void register(DbRequest request)
    {
        queue.AddLast(request);
    }

    public void syncChanges()
    {
        Console.WriteLine($"[NHIBERNATE SESSION '{configFileName}'] Committing {queue.Count} requests");

        try
        {
            if (!session.GetCurrentTransaction().IsActive)
                session.BeginTransaction();

            foreach (var request in queue)
            {
                execute(request);
            }
            
            session.GetCurrentTransaction().Commit();
            session.Clear();
            queue.Clear();
        }
        catch (Exception e)
        {
            Console.WriteLine($"[NHIBERNATE SESSION '{configFileName}'] Could not commit changes. Details: {e.Message}");
            Console.WriteLine(e);
            session.GetCurrentTransaction().Rollback();
            session.Clear();
            queue.Clear();
        }
    }

    public override void reconnect()
    {
        if (session != null)
        {
            session.SessionFactory.Close();
            session.Close();
        }
        
        if (interceptedSession != null)
        {
            interceptedSession.SessionFactory.Close();
            interceptedSession.Close();
        }
        
        try
        {
            Connect();
            if (isHealthy())
                syncChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private bool isHealthy()
    {
        try
        {
            if (session.IsOpen)
                session.FlushMode = FlushMode.Commit;
            
            var result = session.CreateSQLQuery("SELECT 1").UniqueResult();
            
            session.FlushMode = FlushMode.Auto;
            
            if (interceptedSession.IsOpen)
                interceptedSession.FlushMode = FlushMode.Commit;
            
            var interceptedResult = interceptedSession.CreateSQLQuery("SELECT 1").UniqueResult();
            
            interceptedSession.FlushMode = FlushMode.Auto;
            
            return result != null && result.Equals(1) && interceptedResult != null && interceptedResult.Equals(1);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public override void markAsUsed()
    {
        this.isUsed = true;
    }

    public override void markAsUnused()
    {
        this.isUsed = false;
    }

    public override ISession getConnection()
    {
        return this.interceptedSession;
    }
}