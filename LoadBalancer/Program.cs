﻿using LoadBalancer.Core;
using LoadBalancer.Core.Session;
using LoadBalancer.DataBase.Entities;
using NHibernate;

namespace LoadBalancer;

internal static class Program
{
    private static void Main(string[] args)
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

        DatabaseSession[] sessions = sessionsFactory.createSessions(configFileNames);
        loadBalancer.injectSessions(sessions);
        
        ISession session = loadBalancer.connection<ISession>();
        session.BeginTransaction();

        User user = new()
        {
            Id = 1,
            Name = "John1",
            Email = "john1@gmail.com",
            Sex = "Male"
        };

        session.Save(user);
        session.GetCurrentTransaction().Commit();
    }
}