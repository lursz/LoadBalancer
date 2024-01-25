using NHibernate;

namespace LoadBalancer.Core.Session;


// DatabaseSessions factory
public class SessionsFactory
{
    private LoadBalancer<DatabaseSession, ISession> loadBalancer;
    public SessionsFactory(LoadBalancer<DatabaseSession, ISession> loadBalancer)
    {
        this.loadBalancer = loadBalancer;
    }

    public DatabaseSession[] createSessions(string[] configFileNames)
    {
        DatabaseSession[] sessionsArray = new DatabaseSession[configFileNames.Length];
        
        int i = 0;
        foreach (string configFileName in configFileNames)
        {
            LoadBalancerInterceptor interceptor = new LoadBalancerInterceptor(loadBalancer);
            var session = new DatabaseSession(interceptor, configFileName);
            sessionsArray[i] = session;
            i++;
        }
        
        return sessionsArray;       
    }
}