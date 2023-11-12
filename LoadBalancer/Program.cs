using System.Reflection;
using LoadBalancer.db;
using LoadBalancer.Models;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace LoadBalancer;

internal static class Program
{
    private static void Main(string[] args)
    {

        Reader reader = new Reader();

        var config = new Configuration();
        config.DataBaseIntegration(d =>
        {
            d.ConnectionString = reader.DBsConnectionStrings?[0].ConnectionString;
            d.Dialect<MsSql2012Dialect>(); //prawdopodobnie trzeba zmienic
            d.Driver<SqlClientDriver>(); //prawdopodobnie trzeba zmienic
        });

        config.AddAssembly(Assembly.GetExecutingAssembly());
        var sessionFactory = config.BuildSessionFactory();

        using (var session = sessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var user = new User(1, "asdf", "password", "email@email.email");
            session.Save(user);
            transaction.Commit();
        }

    }
}
