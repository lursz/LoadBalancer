using LoadBalancer.Core;

namespace LoadBalancer.Abstracts;

public interface ILoadBalanceAlgorithm
{
    public DatabaseSession chooseSession(DatabaseSession[] sessions);
}