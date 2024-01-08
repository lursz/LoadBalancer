using LoadBalancer.Core;

namespace LoadBalancer.Abstracts;

public abstract class ManageableSession
{

    public enum Status { UP, DOWN }
    public Status status = Status.DOWN;
    public bool isUsed = false;
    private object session;

    public abstract void execute(DbRequest request);

    public abstract void reconnect();

    public abstract void markAsUsed();
    
    public abstract void markAsUnused();
    
    public abstract object getConnection();
}