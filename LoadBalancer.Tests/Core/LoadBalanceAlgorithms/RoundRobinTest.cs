using System;
using JetBrains.Annotations;
using LoadBalancer.Abstracts;
using LoadBalancer.Core.LoadBalanceAlgorithms;
using LoadBalancer.Core.Session;
using LoadBalancer.Core;
using Xunit;

namespace LoadBalancer.Tests.Core.LoadBalanceAlgorithms;

public class RoundRobinTest : IDisposable
{
    ILoadBalanceAlgorithm<DatabaseSession> loadBalanceAlgorithm = new RoundRobin<DatabaseSession>();
    DatabaseSession[] sessions;
    
    public RoundRobinTest()
    {
        LoadBalancer<DatabaseSession> loadBalancer = new(loadBalanceAlgorithm);
        SessionsFactory sessionsFactory = new SessionsFactory(loadBalancer);
        
        string[] configFileNames =
        {
            "./Configs/config1.cfg.xml",
            "./Configs/config2.cfg.xml",
            "./Configs/config3.cfg.xml",
        };
        
        sessions = sessionsFactory.createSessions(configFileNames);
        // foreach (var session in sessions)
        // {
        //     session.markAsUnused();
        //     session.state = new State(new Up());
        // }
    }

    [Fact]
    public void testChooseSession()
    {
        int testRange = 10;
        for (var i = 0; i < testRange; i++)
        {
            var chosenSession = loadBalanceAlgorithm.chooseSession(sessions);
            Assert.NotNull(chosenSession);
            Assert.Equal(sessions[i % sessions.Length], chosenSession);
        }
    }

    public void Dispose(){}
}