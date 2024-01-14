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

            Abstracts.ILoadBalanceAlgorithm<DatabaseSession> loadBalanceAlgorithm = new Core.LoadBalanceAlgorithms.Random<DatabaseSession>();
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
            
            ISession session = loadBalancer.connection<ISession>();
            session.BeginTransaction();

            User user = new()
            {
                Id = 1,
                Name = "Elo",
                Email = "rafal@gmail.com",
                Sex = "Male"
            };
            
            // CREATE
            // session.Save(user);
            // session.GetCurrentTransaction().Commit();
            
            // DELETE
            // session.Delete(user);
            // session.GetCurrentTransaction().Commit();
            
            // UPDATE
            // session.Update(user);
            // session.GetCurrentTransaction().Commit();

            // SELECT
            var users = session.CreateQuery("from User").List<User>();
            foreach (var u in users)
            {
                Console.WriteLine(u.Name);
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}