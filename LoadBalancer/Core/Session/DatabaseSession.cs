using System.Reflection;
using LoadBalancer.Abstracts;
using LoadBalancer.DataBase.Entities;
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
            Console.WriteLine($"COULD NOT CONNECT TO DATABASE {configFileName}");
            Console.WriteLine(e);
        }
        this.queue = new LinkedList<DbRequest>();
    }
    
    private void Connect()
    {
        session = new Configuration().Configure(this.configFileName).BuildSessionFactory().OpenSession();
        interceptedSession = new Configuration().Configure(this.configFileName).BuildSessionFactory().WithOptions().Interceptor(interceptor).OpenSession();
        status = Status.UP;
        session.CacheMode = CacheMode.Ignore;
        interceptedSession.CacheMode = CacheMode.Ignore;
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

    public void PrintObjectProperties(object obj)
    {
        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            object value = property.GetValue(obj);

            Console.WriteLine($"{property.Name}: {value}");
        }
    }

    public override void execute(DbRequest request)
    {
        try
        {
            // Do nothing on SELECT
            if (request.getType() == DbRequest.Type.SELECT) return;

            if (status == Status.DOWN)
            {
                // TODO: Register request in a queue
                register(request);
                Console.WriteLine($"DATABASE IS DOWN, REQUEST ADDED TO THE QUEUE: {queue.Count}");
                reconnect();
                return;
            }

            if (status == Status.SYNC)
            {
                Console.WriteLine($"DATABASE IS SYNCING, REQUEST ADDED TO THE QUEUE: {queue.Count}");
            }
            

            if (!session.GetCurrentTransaction().IsActive)
                session.BeginTransaction();
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
            session.GetCurrentTransaction().Commit();
            session.Evict(request.getObject());
            session.Flush();
            session.Clear();
        }
        catch (NotSupportedException exception)
        {
            
            Console.WriteLine($"[NHIBERNATE SESSION '{configFileName}'] Could not execute request. Details: {exception.Message}");
            throw;
        }
        catch(Exception exception)
        {
            Console.WriteLine("COULD NOT SEND REQUEST, DATABASE IS NOT ACTIVE");
            Console.WriteLine(exception);
            // Close();
            // TODO: Set status to down
            status = Status.DOWN;
            // TODO: Register request in a queue
            register(request);
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
        Console.WriteLine($"[NHIBERNATE SESSION SYNC CHANGES'{configFileName}'] Committing {queue.Count} requests");

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
            Console.WriteLine("COULD NOT RECONNECT");
        }
    }
    
    public bool isHealthy()
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
            Console.WriteLine(interceptedResult);
            interceptedSession.FlushMode = FlushMode.Auto;
            
            return result != null && result.Equals(1) && interceptedResult != null && interceptedResult.Equals(1);

        }
        catch (Exception e)
        {
            // Console.WriteLine(e);
            return false;
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