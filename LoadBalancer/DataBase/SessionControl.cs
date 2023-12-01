using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Tool.hbm2ddl;

namespace LoadBalancer.DataBase;

public class SessionControl
{
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        // var connectionString = configuration.GetConnectionString("DefaultConnection");
        var connectionString = "Host=localhost;Port=5432;Username=user1;Password=password1;Database=database1";
        var sessionFactory = Fluently.Configure()
            .Database(MsSqlConfiguration.MsSql2012
                .ConnectionString(connectionString))
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TodoMap>())
            .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true))
            .BuildSessionFactory();

        services.AddSingleton(sessionFactory);
        services.AddScoped(factory => sessionFactory.OpenSession());
    }
}