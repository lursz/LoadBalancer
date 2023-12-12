using NHibernate;
using NHibernate.Cfg;

namespace LoadBalancer.DataBase;

public static class NHibernateHelper
{
    private static ISessionFactory _sessionFactory;

    internal static ISessionFactory SessionFactory
    {
        get
        {
            if (_sessionFactory != null) return _sessionFactory;
            
            var configuration = new Configuration().Configure();
            configuration.Interceptor = new Interceptor();
            // configuration.SetInterceptor(new Interceptor());
            var schemaExport = new NHibernate.Tool.hbm2ddl.SchemaExport(configuration);
            schemaExport.Create(false, true);   // false, true - do not print to console, execute in db
            _sessionFactory = configuration.BuildSessionFactory();
            return _sessionFactory;
        }
    }

    public static ISession OpenSession()
    {
        return SessionFactory.OpenSession();
    }
}