using LoadBalancer.Abstracts;

namespace LoadBalancer.Core.LoadBalanceAlgorithms;
public class RoundRobin : ILoadBalanceAlgorithm
{
    private int _index = 0;
    public DatabaseSession chooseSession(DatabaseSession[] sessions)
    {
        var attempts = sessions.Length;
        while (attempts > 0)
        {
            var session = sessions[_index];
            _index = (_index + 1) % sessions.Length;
            if (session.status == ManageableSession.Status.UP && session.isUsed() == false)
                return session;
            attempts--;
        }
        throw new InvalidOperationException("RoundRobin: Failed to choose a suitable session.");
    }
}