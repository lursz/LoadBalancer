using LoadBalancer.Abstracts;

namespace LoadBalancer.Core.LoadBalanceAlgorithms;
public class RoundRobin : LoadBalanceAlgorithm
{
    public ManageableSession chooseSession(ManageableSession[] sessions)
    {
        throw new NotImplementedException();
    }
}