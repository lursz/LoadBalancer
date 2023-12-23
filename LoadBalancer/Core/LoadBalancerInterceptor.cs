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
        _loadBalancer.redirect(new DbRequest(entity, DbRequestType.Save));
    }
    public override void OnDelete(object entity)
    {
        _loadBalancer.redirect(new DbRequest(entity, DbRequestType.Delete));
    }
    public override void OnUpdate(object entity)
    {
        _loadBalancer.redirect(new DbRequest(entity, DbRequestType.Update));
    }
    public override void OnLoad(object entity)
    {
        _loadBalancer.redirect(new DbRequest(entity, DbRequestType.Load));
    }
    
    
}
