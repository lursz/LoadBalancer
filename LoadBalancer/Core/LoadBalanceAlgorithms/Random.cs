using LoadBalancer.Abstracts;

namespace LoadBalancer.Core.LoadBalanceAlgorithms;

public class Random<Session> : ILoadBalanceAlgorithm<Session> where Session : ManageableSession
{
    private readonly int _allowedAttempts = 10;
    public Session chooseSession(Session[] sessions)
    {
        var attempts = _allowedAttempts;
        while (attempts > 0)
        {
            var session = sessions[0];
            if (session.status == ManageableSession.Status.UP && session.isUsed == false)
                return session;
            attempts--;
        }
        throw new InvalidOperationException("Random: Failed to choose a suitable session.");
    }
}