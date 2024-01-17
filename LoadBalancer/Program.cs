using LoadBalancer.Connection;
using LoadBalancer.Core;
using LoadBalancer.Core.Session;
using LoadBalancer.DataBase.Entities;
using NHibernate;

namespace LoadBalancer;

internal static class Program
{
    private static void Main(string[] args)
    {
        try
        {

            Abstracts.ILoadBalanceAlgorithm<DatabaseSession> loadBalanceAlgorithm = new Core.LoadBalanceAlgorithms.RoundRobin<DatabaseSession>();
            LoadBalancer<DatabaseSession> loadBalancer = new(loadBalanceAlgorithm);

            SessionsFactory sessionsFactory = new SessionsFactory(loadBalancer);

            string[] configFileNames =
            {
                "./Configs/config1.cfg.xml",
                "./Configs/config2.cfg.xml",
                "./Configs/config3.cfg.xml",
            };
            
            // Migration migration = new(configFileNames);
            // migration.DropAndMigrateAll();
            // migration.MigrateAll();

            DatabaseSession[] sessions = sessionsFactory.createSessions(configFileNames);
            loadBalancer.injectSessions(sessions);
            
        

            // User user = new()
            // {
            //     Id = 1,
            //     Name = "Test",
            //     Email = "test@gmail.com",
            //     Sex = "Male"
            // };
            
            // session.BeginTransaction();

            // // CREATE
            // session.Save(user);
            // session.GetCurrentTransaction().Commit();

            // list with user names

            for (int i = 0; i < 3; i++)
            {
                try {
                    Console.WriteLine($"REQUEST: {i}");
                    ISession session = loadBalancer.connection<ISession>();
                    User user = new()
                    {
            
                        Name = "He",
                        Email = "john@gmail.com",
                        Sex = "Male"
                    };
                
                    session.BeginTransaction();
                    session.Save(user);
                    session.GetCurrentTransaction().Commit();
                    // session.Clear();
                
                } catch (Exception exception) {
                    Console.WriteLine(exception);
                }
            }
            
            // DELETE
            // session.Delete(user);
            // session.GetCurrentTransaction().Commit();
            
            // UPDATE
            // session.Update(user);
            // session.GetCurrentTransaction().Commit();

            // SELECT
            // var users = session.CreateQuery("from User").List<User>();
            // foreach (var u in users)
            // {
            //     Console.WriteLine(u.Name);
            // }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}