using Xunit;
using System;
using LoadBalancer.Abstracts;
using LoadBalancer.Core;
using LoadBalancer.Core.LoadBalanceAlgorithms;
using LoadBalancer.Core.Session;

namespace LoadBalancer.Tests.Core.LoadBalanceAlgorithms;

public class RandomTest : IDisposable
{
    ILoadBalanceAlgorithm<DatabaseSession> loadBalanceAlgorithm = new Random<DatabaseSession>();
    DatabaseSession[] sessions;
    
    public RandomTest()
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
        var chosenSession = this.loadBalanceAlgorithm.chooseSession(sessions);
        Assert.NotNull(chosenSession);
        
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
    

    public void Dispose(){}
}