using LoadBalancer.Core;

namespace LoadBalancer.Abstracts;

public abstract class ManageableSession
{

    public enum Status { UP, DOWN }

    public abstract Object execute(DbRequest request);

    public abstract bool isUsed();

    public abstract void fix();



}