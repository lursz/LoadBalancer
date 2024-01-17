using LoadBalancer.Abstracts;

namespace LoadBalancer.Core.LoadBalanceAlgorithms;

public class RoundRobin<Session> : ILoadBalanceAlgorithm<Session> where Session : ManageableSession
{
    private static int _index = 0;

    public Session chooseSession(Session[] sessions)
    {
        for (var i = 0; i < sessions.Length; i++)
        {
            var index = (_index + i) % sessions.Length;
            var session = sessions[index];

            if (session.status == ManageableSession.Status.UP && session.isUsed == false)
                return session;
        }
        throw new InvalidOperationException("RoundRobin: Failed to choose a suitable session.");
    }
}