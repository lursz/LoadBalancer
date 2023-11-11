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
        List<DBInstance> dbInstances = DBHandler.DbInstances;
        foreach (var dbInstance in dbInstances)
        {
            Console.WriteLine(dbInstance.Name);
            Console.WriteLine(dbInstance.ConnectionString);
        }


        var config = new Configuration();
        config.DataBaseIntegration( d=> {
            d.ConnectionString = dbInstances[0].ConnectionString;
            d.Dialect<MsSql2012Dialect>(); //prawdopodobnie trzeba zmienic
            d.Driver<SqlClientDriver>(); //prawdopodobnie trzeba zmienic
        });

        config.AddAssembly(Assembly.GetExecutingAssembly());
        var sessionFactory = config.BuildSessionFactory();

        using (var session = sessionFactory.OpenSession())
        {
            var user = new User(1, "asdf", "password", "email@email.email");
            session.Save(user);
        }


    }
}