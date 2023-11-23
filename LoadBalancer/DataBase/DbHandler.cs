// using LoadBalancer.DataBase.Connection;
// using Microsoft.EntityFrameworkCore;

// namespace LoadBalancer.DataBase;

// public static class DbHandler
// {

//     public static void MigrateDBs()
//     {
//         if (Reader.DBsConnectionStrings == null) return;
//         foreach (var dbConnectionString in Reader.DBsConnectionStrings)
//         {
//             var context = new Context(dbConnectionString.ConnectionString);
//             context.Database.Migrate();
//         }
//     }
    
    
//     public static void Create<T>(T obj) where T : class
//     {
//         if (Reader.DBsConnectionStrings == null) return;
//         foreach (var dbConnectionString in Reader.DBsConnectionStrings)
//         {
//             var context = new Context(dbConnectionString.ConnectionString);
//             context.Add(obj);
//             context.SaveChanges();
//         }
//     }
    
//     public static void Update<T>(T obj) where T : class
//     {
//         if (Reader.DBsConnectionStrings == null) return;
//         foreach (var dbConnectionString in Reader.DBsConnectionStrings)
//         {
//             var context = new Context(dbConnectionString.ConnectionString);
//             context.Update(obj);
//             context.SaveChanges();
//         }
//     }
    
//     public static void Delete<T>(T obj) where T : class
//     {
//         if (Reader.DBsConnectionStrings == null) return;
//         foreach (var dbConnectionString in Reader.DBsConnectionStrings)
//         {
//             var context = new Context(dbConnectionString.ConnectionString);
//             context.Remove(obj);
//             context.SaveChanges();
//         }
//     }
    
//     public static object? Select(string linqQuery)
//     {
        
//         if (Reader.DBsConnectionStrings == null) return null;
//         var context = new Context(Reader.DBsConnectionStrings.First().ConnectionString);
//         return context.Database.ExecuteSqlRaw(linqQuery);
//     }



// }