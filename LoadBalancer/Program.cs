﻿using LoadBalancer.Connection;
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

            try 
            {
                Migration migration = new(configFileNames);
                migration.DropAndMigrateAll();
                migration.MigrateAll();
            }
            catch (Exception e)
            {
                // ignored
            }


            DatabaseSession[] sessions = sessionsFactory.createSessions(configFileNames);
            loadBalancer.injectSessions(sessions);


            while (true) {
                try {
                    Console.WriteLine($" >> List of commands:\t 1 - Select\t 2 - List all users\t 3 - Insert\t 4 - Insert random user\t 5 - Delete\t 6 - Update\t 7 - Change LoadBalancing algorithm >> ");
                    string userInput = Console.ReadLine();
                    ISession session = loadBalancer.connection<ISession>();

                    switch (userInput) 
                    {
                        case "1": // Select
                            Console.WriteLine("Enter user ID: ");
                            var userID = Console.ReadLine();
                            var selectedUser = session.Get<User>(int.Parse(userID));
                            Console.WriteLine($"{selectedUser.Name}, {selectedUser.Email}, {selectedUser.Sex})");
                            break;
                        
                        case "2": //List all users
                            var allUsers = session.CreateQuery("from User").List<User>();
                            foreach (var u in allUsers)
                            {
                                Console.WriteLine($"{u.Id} {u.Name} {u.Email} {u.Sex}");
                            }
                            break;
                        
                        case "3": // Insert
                            User insertUser = getUserFromKeyboard();
                            session.BeginTransaction();
                            session.Save(insertUser);
                            session.GetCurrentTransaction().Commit();
                            session.Flush();
                            session.Clear();
                            session.Evict(insertUser);
                            Console.WriteLine("Added new user");
                            break;
                        
                        case "4": //Insert random user
                            
                            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                            Random random = new();
                            string randomName = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
                            string randomEmail = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
                            
                            User randomUser = new()
                                {
                                    Name = randomName,
                                    Email = randomEmail,
                                    Sex = "Female"
                                };
                            session.BeginTransaction();
                            session.Save(randomUser);
                            session.GetCurrentTransaction().Commit();
                            session.Flush();
                            session.Clear();
                            session.Evict(randomUser);
                            Console.WriteLine("Added new user");
                            
                            break;
                        
                        case "5": // Delete
                            Console.WriteLine("Enter ID to delete: ");
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
                        
                        case "6": // Update
                            Console.WriteLine("Enter ID to update: ");
                            string updateId = Console.ReadLine();
                            var userToUpdate = session.Get<User>(int.Parse(updateId));
                            Console.WriteLine("Enter new name: ");
                            var userNewName = Console.ReadLine();
                            
                            userToUpdate.Name = userNewName;
                            session.BeginTransaction();
                            session.Update(userToUpdate);
                            session.GetCurrentTransaction().Commit();
                            session.Flush();
                            session.Clear();
                            session.Evict(userToUpdate);
                            Console.WriteLine("Updated user");
                            break;
                        
                        case "7": //Change LoadBalancing algorithm
                            Console.WriteLine("Choose algorithm: 1 - RoundRobin, 2 - Random");
                            string algorithm = Console.ReadLine();
                            switch (algorithm) {
                                case "1":
                                    loadBalanceAlgorithm = new Core.LoadBalanceAlgorithms.RoundRobin<DatabaseSession>();
                                    loadBalancer.changeLoadBalanceAlgorithm(loadBalanceAlgorithm);
                                    Console.WriteLine("Algorithm changed to RoundRobin");
                                    break;
                                case "2":
                                    loadBalanceAlgorithm = new Core.LoadBalanceAlgorithms.Random<DatabaseSession>();
                                    loadBalancer.changeLoadBalanceAlgorithm(loadBalanceAlgorithm);
                                    Console.WriteLine("Algorithm changed to Random");
                                    break;
                                default:
                                    Console.WriteLine("Invalid input");
                                    break;
                            }
                            break;
                        
                        default:
                            Console.WriteLine("Invalid input");
                            break;
                    }
                
                } catch (Exception exception) {
                    Console.WriteLine(exception);
                }
            }
            
            
            User getUserFromKeyboard()
            {
                User tempUser = new();
                Console.WriteLine("Enter user name");
                tempUser.Name = Console.ReadLine();
                Console.WriteLine("Enter email");
                tempUser.Email = Console.ReadLine();
                Console.WriteLine("Enter gender");
                tempUser.Sex = Console.ReadLine();
                return tempUser;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }


    
}