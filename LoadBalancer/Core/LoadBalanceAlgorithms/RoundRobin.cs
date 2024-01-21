using LoadBalancer.Abstracts;

namespace LoadBalancer.Core.LoadBalanceAlgorithms;

public class RoundRobin<Session> : ILoadBalanceAlgorithm<Session> where Session : ManageableSession
{
    private static int prevIndex = -1;

    public Session chooseSession(Session[] sessions)
    {
        for (var i = 0; i < sessions.Length; i++)
        {
            int currentIndex = (prevIndex + 1 + i) % sessions.Length;
            var session = sessions[currentIndex];

            if (session.status == ManageableSession.Status.UP && session.isUsed == false)
            {
                Console.WriteLine($"RETURNED SESSION WITH INDEX {currentIndex}");
                prevIndex = currentIndex;
                return session;
            }
        }
        throw new InvalidOperationException("RoundRobin: Failed to choose a suitable session.");
    }
}