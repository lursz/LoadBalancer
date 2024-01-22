using LoadBalancer.Abstracts;

namespace LoadBalancer.Core.LoadBalanceAlgorithms;

public class Random<Session> : ILoadBalanceAlgorithm<Session> where Session : ManageableSession
{
    public Session chooseSession(Session[] sessions)
    {
        var randomIndex = new System.Random().Next(0, sessions.Length);
        for (var i = 0; i < sessions.Length; i++)
        {
            var index = (i + randomIndex) % sessions.Length;
            Console.WriteLine($"CHOOSED SESSION: {index}");
            var session = sessions[index];
            if (session.state.status() == Status.UP && session.isUsed == false)
                return session;
        }
        throw new InvalidOperationException("Random: Failed to choose a suitable session.");
    }
}