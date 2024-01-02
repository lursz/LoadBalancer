using LoadBalancer.Abstracts;

namespace LoadBalancer.Core.LoadBalanceAlgorithms;
public class RoundRobin<Session> : ILoadBalanceAlgorithm<Session> where Session : ManageableSession
{
    private int _index = 0;
    private readonly int _allowedAttempts = 3;
    public Session chooseSession(Session[] sessions)
    {
        var attempts = _allowedAttempts;
        for (int i = _index % sessions.Length; i < sessions.Length; i++)
        {
            var session = sessions[i];
            while (attempts > 0)
            {
                if (session.status == ManageableSession.Status.UP && session.isUsed == false)
                    return session;
                attempts--;
            }
        }
        throw new InvalidOperationException("RoundRobin: Failed to choose a suitable session.");
    }
}
