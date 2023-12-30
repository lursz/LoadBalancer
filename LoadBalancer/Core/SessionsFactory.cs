using LoadBalancer.Connection;

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
        LoadBalancerInterceptor interceptor = new LoadBalancerInterceptor(loadBalancer);
        DatabaseSession[] sessionsArray = new DatabaseSession[Reader.DBsConnectionStrings.Count];
        foreach (var connString in Reader.DBsConnectionStrings)
        {
            var session = new DatabaseSession(interceptor, connString);
            sessionsArray[Reader.DBsConnectionStrings.IndexOf(connString)] = session;
        }
        
        return sessionsArray;       
    }
}