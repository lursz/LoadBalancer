using LoadBalancer.Abstracts;

namespace LoadBalancer.Core;

public class LoadBalancer<Session>(ILoadBalanceAlgorithm<Session> loadBalancerAlgorithm)
    where Session : ManageableSession
{
    private Session[] sessions;

    private Session mainSession;

    public void injectSessions(Session[] sessions)
    {
        this.sessions = sessions;
    }

    public void redirect(DbRequest request) 
    {
        validateSessions();

        Console.WriteLine($"[LOAD BALANCER] Redirecting request: {request}");

        foreach (var session in this.sessions)
        {
            try {
                if (session.isUsed) continue;
                Console.WriteLine("EXECUTE SESSION");
                session.execute(request);

            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }
    }

    public T connection<T>() where T : class
    {
        validateSessions();
        
        if (mainSession != null)
        {
            mainSession.markAsUnused();
        }
        // TODO: Choose session using load balancing algorithm
        mainSession = loadBalancerAlgorithm.chooseSession(this.sessions);
        // mainSession.getConnection();
        mainSession.markAsUsed();
        return mainSession.getConnection() as T;
    }

    private void validateSessions()
    {
        if (this.sessions == null || this.sessions.Length == 0)
        {
            throw new Exception("Sessions not injected");
        }
    }
}