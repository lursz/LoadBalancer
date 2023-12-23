namespace LoadBalancer;

internal static class Program
{
    private static void Main(string[] args)
    {
        Abstracts.LoadBalanceAlgorithm loadBalanceAlgorithm = new Core.LoadBalanceAlgorithms.Random();
        LoadBalancer loadBalancer = new(loadBalanceAlgorithm);

        Abstracts.ManageableSession[] sessions = new Abstracts.ManageableSession[3];
        loadBalancer.injectSessions(sessions);

        // TODO: Execute queries
    }
}