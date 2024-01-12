using LoadBalancer.Abstracts;

namespace LoadBalancer.Core.Session;

public class LoadBalancerInterceptor : DatabaseInterceptor
{
    protected readonly LoadBalancer<DatabaseSession> _loadBalancer;

    public LoadBalancerInterceptor(LoadBalancer<DatabaseSession> loadBalancer)
    {
        _loadBalancer = loadBalancer;
    }

    public override void OnSave(object entity)
    {
        _loadBalancer.redirect(new DbRequest(entity, DbRequest.Type.INSERT));
    }
    public override void OnDelete(object entity)
    {
        _loadBalancer.redirect(new DbRequest(entity, DbRequest.Type.DELETE));
    }
    public override void OnUpdate(object entity)
    {
        _loadBalancer.redirect(new DbRequest(entity, DbRequest.Type.UPDATE));
    }
    public override void OnLoad(object entity)
    {
        _loadBalancer.redirect(new DbRequest(entity, DbRequest.Type.SELECT));
    }
    
    
}
