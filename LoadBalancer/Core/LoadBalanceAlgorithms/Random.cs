using LoadBalancer.Abstracts;

namespace LoadBalancer.Core.LoadBalanceAlgorithms;

public class Random<Session> : ILoadBalanceAlgorithm<Session> where Session : ManageableSession
{
    private readonly int _allowedAttempts = 2;
    public Session chooseSession(Session[] sessions)
    {
        Session[] sessionsCopy = new Session[sessions.Length];
        sessions.CopyTo(sessionsCopy, 0);
        
        var randomIndex = new System.Random().Next(0, sessionsCopy.Length);
        for (int i = randomIndex; i < sessionsCopy.Length; i++)
        {
            if (sessionsCopy[i] == null)
                continue;
            
            var session = sessionsCopy[i];
            var attempts = _allowedAttempts;
            while (attempts > 0)
            {
                if (session.status == ManageableSession.Status.UP && session.isUsed == false)
                    return session;
                attempts--;
            }
            sessionsCopy[i] = null;
        }
        
        throw new InvalidOperationException("Random: Failed to choose a suitable session.");
    }
}