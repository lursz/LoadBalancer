

public class LoadBalancer(LoadBalanceAlgorithm loadBalancerAlgorithm)
{
    private ManageableSession[] sessions;
    private LoadBalanceAlgorithm loadBalancerAlgorithm = loadBalancerAlgorithm;

    public void injectSessions(ManageableSession[] sessions)
    {
        this.sessions = sessions;
    }

    public void redirect() 
    {
        if (this.sessions == null)
        {
            throw new Exception("Sessions not injected");
        }

        foreach (var session in this.sessions)
        {
            try {
                // TODO: execute request on each session
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }
    }

    public void connection()
    {
        // TODO: Choose session using load balancing algorithm
        // TODO: Mark session as used
        // TODO: Return session
    }
}