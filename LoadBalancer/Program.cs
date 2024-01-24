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
            
            Migration migration = new(configFileNames);
            migration.DropAndMigrateAll();
            migration.MigrateAll();

            DatabaseSession[] sessions = sessionsFactory.createSessions(configFileNames);
            loadBalancer.injectSessions(sessions);

            // ISession session = loadBalancer.connection<ISession>();

            User user = new()
            {
                
                Name = "John",
                Email = "john@gmail.com",
                Sex = "Male"
            };

            while (true) {
                try {
                    string userInput = Console.ReadLine();
                    // Console.WriteLine($"REQUEST: {i}");
                    ISession session = loadBalancer.connection<ISession>();

                    switch (userInput) 
                    {
                        case "1":
                            var users = session.CreateQuery("from User").List<User>();
                            foreach (var u in users)
                            {
                                Console.WriteLine($"{u.Id} {u.Name} {u.Email} {u.Sex}");
                            }
                            break;
                        case "2":
                            session.BeginTransaction();
                            session.Save(user);
                            session.GetCurrentTransaction().Commit();
                            session.Flush();
                            session.Clear();
                            session.Evict(user);
                            Console.WriteLine("Added new user");
                            break;
                        case "3":
                            string deleteId = Console.ReadLine();
                            var userToDelete = session.Get<User>(int.Parse(deleteId));
                            session.BeginTransaction();
                            session.Delete(userToDelete);
                            session.GetCurrentTransaction().Commit();
                            session.Flush();
                            session.Clear();
                            session.Evict(userToDelete);
                            Console.WriteLine("Removed user");
                            break;
                        case "4":
                            User updatedUser = new()
                            {
                                Id = 1,
                                Name = "Bob",
                                Email = "bob@gmail.com",
                                Sex = "Male"
                            };
                            session.Update(updatedUser);
                            session.GetCurrentTransaction().Commit();
                            session.Flush();
                            session.Clear();
                            session.Evict(updatedUser);
                            break;
                        default:
                            Console.WriteLine("Invalid input");
                            break;
                    }


                   
                
                } catch (Exception exception) {
                    Console.WriteLine(exception);
                }
            }

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

            // list with user name
            
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