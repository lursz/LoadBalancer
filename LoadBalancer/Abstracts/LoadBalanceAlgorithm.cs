using LoadBalancer.Core;

namespace LoadBalancer.Abstracts;

public interface ILoadBalanceAlgorithm<Session> where Session : ManageableSession
{
    public Session chooseSession(Session[] sessions);
}