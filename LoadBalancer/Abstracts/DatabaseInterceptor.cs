using NHibernate;
using NHibernate.Type;

namespace LoadBalancer.Abstracts;

public abstract class DatabaseInterceptor : EmptyInterceptor
{
    public abstract void OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types);
    public abstract void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types);
    public abstract void OnUpdate(object entity, object id, object[] state, string[] propertyNames, IType[] types);
    public abstract void OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types);
}