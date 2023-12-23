namespace LoadBalancer.Abstracts;

public interface LoadBalanceAlgorithm
{
    public ManageableSession chooseSession(ManageableSession[] sessions);
}