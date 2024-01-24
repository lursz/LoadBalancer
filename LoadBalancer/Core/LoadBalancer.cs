using LoadBalancer.Abstracts;

namespace LoadBalancer.Core;

public class LoadBalancer<Session>()
    where Session : ManageableSession
{
    private Session[] sessions;

    private Session mainSession;

    private ILoadBalanceAlgorithm<Session> loadBalancerAlgorithm;

    public void injectSessions(Session[] sessions)
    {
        this.sessions = sessions;
    }

    public LoadBalancer(ILoadBalanceAlgorithm<Session> loadBalancerAlgorithm) :this()
    {
        this.loadBalancerAlgorithm = loadBalancerAlgorithm;
    }

    public void changeLoadBalanceAlgorithm(ILoadBalanceAlgorithm<Session> loadBalancerAlgorithm)
    {
        this.loadBalancerAlgorithm = loadBalancerAlgorithm;
    }



    public void redirect(DbRequest request) 
    {
        validateSessions();

        Console.WriteLine($"[LOAD BALANCER] Redirecting request: {request}");

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