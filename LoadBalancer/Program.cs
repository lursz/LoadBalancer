﻿using LoadBalancer.Core;
using NHibernate;

namespace LoadBalancer;

internal static class Program
{
    private static void Main(string[] args)
    {
        Abstracts.ILoadBalanceAlgorithm<DatabaseSession> loadBalanceAlgorithm = new Core.LoadBalanceAlgorithms.Random<DatabaseSession>();
        Core.LoadBalancer<DatabaseSession> loadBalancer = new(loadBalanceAlgorithm);

        Core.SessionsFactory sessionsFactory = new SessionsFactory(loadBalancer);

        string[] configFileNames =
        {
            "./Configs/config1.cfg.xml",
            "./Configs/config2.cfg.xml",
            "./Configs/config3.cfg.xml",
        };

        Core.DatabaseSession[] sessions = sessionsFactory.createSessions(configFileNames);
        loadBalancer.injectSessions(sessions);
        
        ISession session = loadBalancer.connection<ISession>();
        session.BeginTransaction();
    }
}