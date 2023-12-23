namespace LoadBalancer.Abstracts;

public abstract class ManageableSession
{

    public enum Status { UP, DOWN }

    public abstract Object execute();

    public abstract bool isUsed();

    public abstract void fix();



}