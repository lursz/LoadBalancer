using LoadBalancer.Abstracts;

namespace LoadBalancer.Core.LoadBalanceAlgorithms;

public class Random<Session> : ILoadBalanceAlgorithm<Session> where Session : ManageableSession
{
    public Session chooseSession(Session[] sessions)
    {
        if (sessions.Length == 0)
            throw new InvalidOperationException("Random: Cannot choose a session. Please check your configuration.");
        var randomIndex = new System.Random().Next(0, sessions.Length);
        for (var i = 0; i < sessions.Length; i++)
        {
            var index = (i + randomIndex) % sessions.Length;
            var session = sessions[index];
            if (session.state.status() != Status.UP || session.isUsed != false) continue;
            Console.WriteLine($"Random: Chosen session: {index}");
            return session;
        }
        throw new InvalidOperationException("Random: Failed to choose a suitable session.");
    }
}