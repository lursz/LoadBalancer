using LoadBalancer.DataBase.Connection;
using Microsoft.EntityFrameworkCore;

namespace LoadBalancer.DataBase;

public static class BeginMigrationToManyDBs
{

    public static void Start()
    {
        if (Reader.DBsConnectionStrings == null) return;
        foreach (var dbConnectionString in Reader.DBsConnectionStrings)
        {
            var context = new Context(dbConnectionString.ConnectionString);
            context.Database.Migrate();
        }
    }

}