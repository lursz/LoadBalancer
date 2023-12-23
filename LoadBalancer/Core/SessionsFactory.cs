namespace LoadBalancer.Core;

public class SessionsFactory 
{
    private LoadBalancer loadBalancer;
    public SessionsFactory(LoadBalancer loadBalancer)
    {
        this.loadBalancer = loadBalancer;
    }

    public DatabaseSession[] createSessions()
    {
        // TODO: Create interceptor and initialize sessions

        return [];       
    }
}