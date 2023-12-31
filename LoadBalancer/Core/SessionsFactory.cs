using LoadBalancer.Connection;

namespace LoadBalancer.Core;

public class SessionsFactory 
{
    private LoadBalancer loadBalancer;
    public SessionsFactory(LoadBalancer loadBalancer)
    {
        this.loadBalancer = loadBalancer;
    }

    public DatabaseSession[] createSessions(string[] configFileNames)
    {
        LoadBalancerInterceptor interceptor = new LoadBalancerInterceptor(loadBalancer);
        DatabaseSession[] sessionsArray = new DatabaseSession[configFileNames.Length];
        foreach (string configFileName in configFileNames)
        {
            var session = new DatabaseSession(interceptor, configFileName);
            sessionsArray.Append(session);
        }
        
        return sessionsArray;       
    }
}