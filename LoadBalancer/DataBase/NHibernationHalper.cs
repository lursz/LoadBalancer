
using NHibernate;
using NHibernate.Cfg;

namespace LoadBalancer.DataBase
{
    public static class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    var configuration = new Configuration();
                    configuration.Configure();
                    _sessionFactory = configuration.BuildSessionFactory();
                    var connString = Connection.Reader.DBsConnectionStrings?[0].ConnectionString;
                    configuration.SetProperty("connection.connection_string", connString);
                }
                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
