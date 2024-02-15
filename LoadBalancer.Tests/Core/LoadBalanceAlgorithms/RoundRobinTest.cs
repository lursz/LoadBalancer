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
        LoadBalancer<DatabaseSession> loadBalancer = new LoadBalancer<DatabaseSession>(loadBalanceAlgorithm);
        SessionsFactory sessionsFactory = new SessionsFactory(loadBalancer);

        string[] configFileNames =
        [
            "./Configs/config1.cfg.xml",
            "./Configs/config2.cfg.xml",
            "./Configs/config3.cfg.xml"
        ];

        sessions = sessionsFactory.createSessions(configFileNames);
        
    }

    [Fact]
    public void testChooseSession()
    {
        foreach (var session in sessions)
        {
            session.markAsUnused();
            session.state = new State(new Up());
        }
        int testRange = 10;
        for (var i = 0; i < testRange; i++)
        {
            var chosenSession = loadBalanceAlgorithm.chooseSession(sessions);
            Assert.Equal(sessions[i % sessions.Length], chosenSession);
        }
    }
    
    [Fact]
    public void testChooseSessionWithNoSessions()
    {
        DatabaseSession[] emptySessions = Array.Empty<DatabaseSession>();
        Assert.Throws<InvalidOperationException>(() => this.loadBalanceAlgorithm.chooseSession(emptySessions));
    }
    
    [Fact]
    public void testChooseSessionWithAllSessionsDown()
    {
        foreach (var session in sessions)
        {
            session.state = new State(new Down());
        }
        Assert.Throws<InvalidOperationException>(() => this.loadBalanceAlgorithm.chooseSession(sessions));
    }
    
    [Fact]
    public void testChooseSessionWithAllSessionsUsed()
    {
        foreach (var session in sessions)
        {
            session.markAsUsed();
        }
        Assert.Throws<InvalidOperationException>(() => this.loadBalanceAlgorithm.chooseSession(sessions));
    }
    
    [Fact]
    public void testChooseSessionWithAllSessionsDownAndUsed()
    {
        foreach (var session in sessions)
        {
            session.state = new State(new Down());
            session.markAsUsed();
        }
        Assert.Throws<InvalidOperationException>(() => this.loadBalanceAlgorithm.chooseSession(sessions));
    }
    

    public void Dispose(){}
}