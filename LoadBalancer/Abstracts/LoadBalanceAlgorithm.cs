#nullable enable          
using LoadBalancer.Core;

namespace LoadBalancer.Abstracts;

public interface ILoadBalanceAlgorithm<Session> where Session : ManageableSession
{
    public Session? ChooseSession(Session[] sessions);
}