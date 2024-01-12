using LoadBalancer.Abstracts;
using NHibernate;
using NHibernate.Type;

namespace LoadBalancer.Core.Session;

public class LoadBalancerInterceptor : EmptyInterceptor
{
    protected readonly LoadBalancer<DatabaseSession> _loadBalancer;

    public LoadBalancerInterceptor(LoadBalancer<DatabaseSession> loadBalancer)
    {
        _loadBalancer = loadBalancer;
    }

    public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
    {
        Console.WriteLine("SAVE INTERCEPTOR");
        _loadBalancer.redirect(new DbRequest(entity, DbRequest.Type.INSERT));
        return false;
    }
    public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types)
    {
        Console.WriteLine("DELETE INTERCEPTOR");
        _loadBalancer.redirect(new DbRequest(entity, DbRequest.Type.DELETE));
    }
    public override bool OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types)
    {   
        Console.WriteLine("LOAD INTERCEPTOR");
        _loadBalancer.redirect(new DbRequest(entity, DbRequest.Type.SELECT));
        return false;
    }
    
    
}
