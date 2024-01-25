#nullable enable
using LoadBalancer.Abstracts;
using NHibernate;

namespace LoadBalancer.Core;

public class LoadBalancer<DBSession, Session>()
    where DBSession : ManageableSession
    where Session : ISession
{
    private DBSession[] sessions;

    private DBSession? mainSession;

    private ILoadBalanceAlgorithm<DBSession> loadBalancerAlgorithm;

    public void InjectSessions(DBSession[] sessions)
    {
        this.sessions = sessions;
    }

    public LoadBalancer(ILoadBalanceAlgorithm<DBSession> loadBalancerAlgorithm) :this()
    {
        this.loadBalancerAlgorithm = loadBalancerAlgorithm;
    }

    public void ChangeLoadBalanceAlgorithm(ILoadBalanceAlgorithm<DBSession> loadBalancerAlgorithm)
    {
        this.loadBalancerAlgorithm = loadBalancerAlgorithm;
    }

    public List<T> GetAllData<T>()
    {   
        Session? session = Connection();
        if (session == null) {
            throw new Exception("No session available");
        }
        var allData = session.CreateQuery("from User").List<T>();
        return (List<T>)allData;
    }

    public void Insert(object objectToInsert)
    {
        Session? session = Connection();

        if (session == null) 
        {
            StoreRequestsInAllSessions(new DbRequest(objectToInsert, DbRequest.Type.INSERT));
            return;
        }

        session.BeginTransaction();
        session.Save(objectToInsert);
        session.GetCurrentTransaction().Commit();
        session.Flush();
        session.Clear();
        session.Evict(objectToInsert);
        Console.WriteLine("Added new user");
    }

    public void Update(object userToUpdate)
    {
        Session? session = Connection();

        if (session == null) 
        {
            StoreRequestsInAllSessions(new DbRequest(userToUpdate, DbRequest.Type.UPDATE));
            return;
        }

        session.BeginTransaction();
        session.Update(userToUpdate);
        session.GetCurrentTransaction().Commit();
        session.Flush();
        session.Clear();
        session.Evict(userToUpdate);
        Console.WriteLine("Updated user");
    }
    

    public void Delete(object objectToDelete)
    {
        Session? session = Connection();

        if (session == null) 
        {
            StoreRequestsInAllSessions(new DbRequest(objectToDelete, DbRequest.Type.DELETE));
            return;
        }

        session.BeginTransaction();
        session.Delete(objectToDelete);
        session.GetCurrentTransaction().Commit();
        session.Flush();
        session.Clear();
        session.Evict(objectToDelete);
        Console.WriteLine("Removed user");
    }   

    public T Select<T>(int id)
    {
        Session? session = Connection();

        if (session == null) 
        {
            throw new Exception("No session available");
        }

        T selectedUser = session.Get<T>(id);
        return selectedUser;
    }

    public void Redirect(DbRequest request) 
    {
        ValidateSessions();

        // Console.WriteLine($"[LOAD BALANCER] Redirecting request: {request}");

        foreach (var session in this.sessions)
        {
            try {
                if (session.isUsed) continue;
                session.execute(request);

            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }
    }

    public Session? Connection()
    {
        ValidateSessions();
        
        if (mainSession != null)
        {
            mainSession.markAsUnused();
        }
        mainSession = loadBalancerAlgorithm.ChooseSession(sessions);

        if (mainSession == null) return default(Session);
        // mainSession.getConnection();
        mainSession.markAsUsed();
        return (Session)mainSession.getConnection();
    }

    private void StoreRequestsInAllSessions(DbRequest request)
    {
        foreach (var session in this.sessions)
        {
            try {
                session.storeObject(request);
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }
    }
    private void ValidateSessions()
    {
        if (this.sessions == null || this.sessions.Length == 0)
        {
            throw new Exception("Sessions not injected");
        }
    }
}