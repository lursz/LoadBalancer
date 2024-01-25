using LoadBalancer.Abstracts;
using NHibernate;
using NHibernate.SqlCommand;
using NHibernate.Type;

namespace LoadBalancer.Core.Session;

public class LoadBalancerInterceptor : EmptyInterceptor
{
    protected readonly LoadBalancer<DatabaseSession, ISession> _loadBalancer;

    public LoadBalancerInterceptor(LoadBalancer<DatabaseSession, ISession> loadBalancer)
    {
        _loadBalancer = loadBalancer;
    }

    public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
    {
        try
        {
            Console.WriteLine("[INTERCEPTOR] ON SAVE INTERCEPTOR");
            Console.WriteLine(entity);
            _loadBalancer.Redirect(new DbRequest(entity, DbRequest.Type.INSERT));
        }
        catch (Exception e)
        {
            Console.WriteLine("[INTERCEPTOR] INTERCEPTOR EXCEPTION");
        }
        return true;
    }
    public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types)
    {
        try
        {
            Console.WriteLine("[INTERCEPTOR] DELETE INTERCEPTOR");
            _loadBalancer.Redirect(new DbRequest(entity, DbRequest.Type.DELETE));

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
    {
        try
        {
            Console.WriteLine("[INTERCEPTOR] FLUSH DIRTY INTERCEPTOR");
            _loadBalancer.Redirect(new DbRequest(entity, DbRequest.Type.UPDATE));
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
        // Console.WriteLine("LOAD INTERCEPTOR");
        _loadBalancer.Redirect(new DbRequest(entity, DbRequest.Type.SELECT));
        return false;
    }
    
}
