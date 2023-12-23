using LoadBalancer.Abstracts;

namespace LoadBalancer.Core.LoadBalanceAlgorithms;

public class Random : LoadBalanceAlgorithm
{
    public ManageableSession chooseSession(ManageableSession[] sessions)
    {
        throw new NotImplementedException();
    }
}