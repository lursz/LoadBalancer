using LoadBalancer.Abstracts;
using NHibernate;
using NHibernate.SqlCommand;
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
        try
        {
            Console.WriteLine("ONSAVE INTERCEPTOR");
            Console.WriteLine(entity);
            _loadBalancer.redirect(new DbRequest(entity, DbRequest.Type.INSERT));
        }
        catch (Exception e)
        {
            Console.WriteLine("INTERCEPTOR EXCEPTION");
        }
        return true;
    }
    public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types)
    {
        Console.WriteLine("ONDELETE INTERCEPTOR");
        try
        {
            Console.WriteLine("DELETE INTERCEPTOR");
            _loadBalancer.redirect(new DbRequest(entity, DbRequest.Type.DELETE));

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
    {
        Console.WriteLine("ONFLUSHDIRTY INTERCEPTOR");
        try
        {
            Console.WriteLine("FLUSHDIRTY INTERCEPTOR");
            _loadBalancer.redirect(new DbRequest(entity, DbRequest.Type.UPDATE));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return false;
    }
    
    public override bool OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types)
    {   
        Console.WriteLine("LOAD INTERCEPTOR");
        _loadBalancer.redirect(new DbRequest(entity, DbRequest.Type.SELECT));
        return false;
    }
    
}
