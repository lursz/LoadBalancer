using LoadBalancer.Abstracts;
using NHibernate.Type;

namespace LoadBalancer.Core;

public class LoadBalancerInterceptor : DatabaseInterceptor
{
    protected readonly LoadBalancer _loadBalancer;

    public LoadBalancerInterceptor(LoadBalancer loadBalancer)
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
