

using NHibernate.Type;

namespace LoadBalancer.Core;

public class LoadBalancerInterceptor : Abstracts.DatabaseInterceptor
{
    public override void OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types)
    {
        throw new NotImplementedException();
    }

    public override void OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
    {
        throw new NotImplementedException();
    }

    public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types)
    {
        throw new NotImplementedException();
    }

    public override void OnUpdate(object entity, object id, object[] state, string[] propertyNames, IType[] types)
    {
        throw new NotImplementedException();
    }
}