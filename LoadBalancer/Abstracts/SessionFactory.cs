using LoadBalancer.Core.Session;

namespace LoadBalancer.Abstracts;

public interface ISessionFactory
{
    public DatabaseSession[] createSessions(string[] configFileNames);
}