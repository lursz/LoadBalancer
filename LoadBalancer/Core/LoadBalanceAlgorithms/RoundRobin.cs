using LoadBalancer.Abstracts;

namespace LoadBalancer.Core.LoadBalanceAlgorithms;
public class RoundRobin<Session> : ILoadBalanceAlgorithm<Session> where Session : ManageableSession
{
    private int _index = 0;
    public Session chooseSession(Session[] sessions)
    {
        var attempts = sessions.Length;
        while (attempts > 0)
        {
            var session = sessions[_index];
            _index = (_index + 1) % sessions.Length;
            if (session.status == ManageableSession.Status.UP && session.isUsed == false)
                return session;
            attempts--;
        }
        throw new InvalidOperationException("RoundRobin: Failed to choose a suitable session.");
    }
}
