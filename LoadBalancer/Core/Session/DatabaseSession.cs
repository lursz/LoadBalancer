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
            this.state = new State(new Up());
        }
        catch (Exception e)
        {
            Console.WriteLine($"COULD NOT CONNECT TO DATABASE {configFileName}");
            // Console.WriteLine(e);
            this.state = new State(new Down());
        }
        this.queue = new LinkedList<DbRequest>();
    }
    
    private void Connect()
    {
        session = new Configuration().Configure(this.configFileName).BuildSessionFactory().OpenSession();
        interceptedSession = new Configuration().Configure(this.configFileName).BuildSessionFactory().WithOptions().Interceptor(interceptor).OpenSession();
        session.CacheMode = CacheMode.Ignore;
        interceptedSession.CacheMode = CacheMode.Ignore;
        Console.WriteLine($"[NHIBERNATE SESSION '{configFileName}'] Connection established");
    }

    private void Close()
    {        
        session.Close();
        session.SessionFactory.Close();
        interceptedSession.Close();
        interceptedSession.SessionFactory.Close();
        this.state.nextState();
        Console.WriteLine($"CURRENT STATE: {this.state.status()}");
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

            if (this.state.status() == Status.DOWN)
            {
                register(request);
                Console.WriteLine($"DATABASE IS {this.state.status()}, REQUEST ADDED TO THE QUEUE: {queue.Count}");
                reconnect();
                return;
            }

            Console.WriteLine($"EXECUTING REQUEST IN STATE: {this.state.status()}");
            

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
            Close();
            register(request);
        }
    }


    public void register(DbRequest request)
    {
        queue.AddLast(request);
    }

    public void syncChanges()
    {
        Console.WriteLine($"[NHIBERNATE SESSION SYNC CHANGES'{configFileName}'] Committing {queue.Count} requests");
        this.state.nextState();
        Console.WriteLine($"CURRENT STATE: {this.state.status()}");
        try
        {
            foreach (var request in queue)
            {
                execute(request);
            }
            
            queue.Clear();
        }
        catch (Exception e)
        {
            Console.WriteLine($"[NHIBERNATE SESSION '{configFileName}'] Could not commit changes. Details: {e.Message}");
            // Console.WriteLine(e);
            session.GetCurrentTransaction().Rollback();
            session.Clear();
            queue.Clear();
        }
    }

    public override void reconnect()
    {   
        try
        {
            Connect();
            if (isHealthy())
                syncChanges();
                this.state.nextState();
        }
        catch (Exception e)
        {
            Console.WriteLine("COULD NOT RECONNECT");
        }
    }
    
    public override bool isHealthy()
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