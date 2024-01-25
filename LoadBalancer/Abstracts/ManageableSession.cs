using LoadBalancer.Core;
using NHibernate;

namespace LoadBalancer.Abstracts;

public abstract class ManageableSession
{

    public State state;
    public bool isUsed = false;
    private object session;

    public abstract void execute(DbRequest request);

    public abstract bool isHealthy();

    public abstract void reconnect();

    public abstract void markAsUsed();
    
    public abstract void markAsUnused();
    
    public abstract ISession getConnection();

    public abstract void storeObject(DbRequest obj);

    public abstract void fix();

}