namespace LoadBalancer;

internal static class Program
{
    private static void Main(string[] args)
    {
        LoadBalanceAlgorithm loadBalanceAlgorithm = new Random();
        LoadBalancer loadBalancer = new(loadBalanceAlgorithm);

        ManageableSession[] sessions = new ManageableSession[3];
        loadBalancer.injectSessions(sessions);

        // TODO: Execute queries
    }
}