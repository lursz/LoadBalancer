using LoadBalancer.Abstracts;

namespace LoadBalancer.Core.LoadBalanceAlgorithms;

public class Random : ILoadBalanceAlgorithm
{
    private readonly int _allowedAttempts = 10;
    public DatabaseSession chooseSession(DatabaseSession[] sessions)
    {
        var attempts = _allowedAttempts;
        while (attempts > 0)
        {
            var session = sessions[new System.Random().Next(0, sessions.Length)];
            if (session.Status == ManageableSession.Status.UP && session.isUsed() == false)
                return session;
            attempts--;
        }
        throw new InvalidOperationException("Random: Failed to choose a suitable session.");
    }
}