using LoadBalancer.Abstracts;

namespace LoadBalancer.Core.LoadBalanceAlgorithms;
public class RoundRobin : ILoadBalanceAlgorithm
{
    public DatabaseSession chooseSession(DatabaseSession[] sessions)
    {
        foreach (var session in sessions)
        {
            if (session.Status == ManageableSession.Status.UP && session.isUsed() == false)
                return session;
        }
        throw new InvalidOperationException("RoundRobin: Failed to choose a suitable session.");
    }
}