using NHibernate.Cfg;

namespace LoadBalancer.Connection;

public class Migration(string[] configFileName)
{

    public void MigrateAll()
    {
        Console.WriteLine("[MIGRATION] Migrating databases unless they exist");
        foreach (var config in configFileName)
        {
            var configuration = new Configuration();
            configuration.Configure(config);

            var schemaUpdate = new NHibernate.Tool.hbm2ddl.SchemaUpdate(configuration);
            schemaUpdate.Execute(false, true);
        }
    }
    public void DropAndMigrateAll()
    {
        foreach (var config in configFileName)
        {
            var configuration = new Configuration();
            configuration.Configure(config);

            var schemaExport = new NHibernate.Tool.hbm2ddl.SchemaExport(configuration);
            schemaExport.Create(false, true);
        }
    }
    
    public void DropAndMigrate(int index)
    {
        var configuration = new Configuration();
        configuration.Configure(configFileName[index]);

        var schemaExport = new NHibernate.Tool.hbm2ddl.SchemaExport(configuration);
        schemaExport.Create(false, true);
    }
}