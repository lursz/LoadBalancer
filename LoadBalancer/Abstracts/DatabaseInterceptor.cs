using NHibernate;
using NHibernate.Type;

namespace LoadBalancer.Abstracts;

public abstract class DatabaseInterceptor : EmptyInterceptor
{
    public abstract void OnSave(object entity);
    public abstract void OnDelete(object entity);
    public abstract void OnUpdate(object entity);
    public abstract void OnLoad(object entity);
}