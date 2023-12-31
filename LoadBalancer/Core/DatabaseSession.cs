using System.Runtime.CompilerServices;
using LoadBalancer.Abstracts;
using LoadBalancer.Connection;
using NHibernate;
using NHibernate.Cfg;

namespace LoadBalancer.Core;

public class DatabaseSession : ManageableSession
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
            this.status = Status.UP;
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
        var sessionFactory = configuration.BuildSessionFactory();
        this.session = sessionFactory.OpenSession();
        this.interceptedSession = sessionFactory.WithOptions().Interceptor(interceptor).OpenSession();
    }

    public override object execute(DbRequest request)
    {
        try
        {
            if (!session.Transaction.IsActive)
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
            
            // session.Transaction.Commit();
            session.GetCurrentTransaction().Commit();
            session.Clear();
            return null;
        }
        catch (NotSupportedException exception)
        {
            status = Status.DOWN;
            register(request);
            Console.WriteLine($"[NHIBERNATE SESSION '{configFileName}'] Could not execute request. Details: {exception.Message}");
            throw;
        }
    }
    
    private void register(DbRequest request)
    {
        queue.AddLast(request);
    }

    public override void fix()
    {
        Console.WriteLine("DatabaseSession fix");
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