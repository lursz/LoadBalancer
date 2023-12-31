using LoadBalancer.Core;

namespace LoadBalancer.Abstracts;

public abstract class ManageableSession
{

    public enum Status { UP, DOWN }
    public Status status = Status.DOWN;

    public bool isUsed = false;

    public abstract Object execute(DbRequest request);

    public abstract void fix();

    public abstract void markAsUsed();
    public abstract void markAsUnused();
    private object session;

    public abstract object getConnection();
}