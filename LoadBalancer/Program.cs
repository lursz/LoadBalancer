namespace LoadBalancer;

internal static class Program
{
    private static void Main(string[] args)
    {
        Abstracts.ILoadBalanceAlgorithm loadBalanceAlgorithm = new Core.LoadBalanceAlgorithms.Random();
        Core.LoadBalancer loadBalancer = new(loadBalanceAlgorithm);

        Core.SessionsFactory sessionsFactory = new(loadBalancer);

        string[] configFileNames =
        [
            "./Configs/config1.cfg.xml",
            "./Configs/config2.cfg.xml",
            "./Configs/config3.cfg.xml",
        ];

        Core.DatabaseSession[] sessions = sessionsFactory.createSessions(configFileNames);

        // log sessions:
        foreach (var session in sessions)
        {
            Console.WriteLine(session);
        }
        
    }
}