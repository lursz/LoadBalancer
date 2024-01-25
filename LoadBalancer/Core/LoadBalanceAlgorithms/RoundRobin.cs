using LoadBalancer.Abstracts;

namespace LoadBalancer.Core.LoadBalanceAlgorithms;

public class RoundRobin<Session> : ILoadBalanceAlgorithm<Session> where Session : ManageableSession
{
    private static int _prevIndex = -1;

    public Session chooseSession(Session[] sessions)
    {
        if (sessions.Length == 0)
            throw new InvalidOperationException("Random: Cannot choose a session. Please check your configuration.");
        for (var i = 0; i < sessions.Length; i++)
        {
            int currentIndex = (_prevIndex + 1 + i) % sessions.Length;
            var session = sessions[currentIndex];

            if (session.state.status() == Status.UP && session.isUsed == false)
            {
                if (!session.isHealthy()) {
                    Console.WriteLine($"Session {currentIndex} is unhealthy, skipping...");
                    continue;
                }

                Console.WriteLine($"RoundRobin: Chosen session: {currentIndex}");
                _prevIndex = currentIndex;
                return session;
            }
        }
        throw new InvalidOperationException("RoundRobin: Failed to choose a suitable session.");
    }
}