using System;
using JetBrains.Annotations;
using LoadBalancer.Core;
using LoadBalancer.Core.LoadBalanceAlgorithms;
using LoadBalancer.Core.Session;
using Xunit;

namespace LoadBalancer.Tests.Core.Session;

public class SessionsFactoryTest : IDisposable
{
    LoadBalancer<DatabaseSession> loadBalancer = new LoadBalancer<DatabaseSession>(new RoundRobin<DatabaseSession>());
    private string[] configFileNames;
    
    public SessionsFactoryTest()
    {
        configFileNames =
        [
            "./Configs/config1.cfg.xml",
            "./Configs/config2.cfg.xml",
            "./Configs/config3.cfg.xml"
        ];
    }

    [Fact]
    public void testCreateSessions()
    {
        DatabaseSession[] sessions = new SessionsFactory(loadBalancer).createSessions(configFileNames);
        Assert.Equal(configFileNames.Length, sessions.Length);
    }
    
    [Fact]
    public void testCreateSessionsWithEmptyConfigFileNames()
    {
        string[] emptyConfigFileNames = Array.Empty<string>();
        DatabaseSession[] sessions = new SessionsFactory(loadBalancer).createSessions(emptyConfigFileNames);
        Assert.Empty(sessions);
    }
    
    [Fact]
    public void testCreateSessionsWithNullConfigFileNames()
    {
        Assert.Throws<NullReferenceException>(() => new SessionsFactory(loadBalancer).createSessions(null));
    }
    
    [Fact]
    public void testCreateSessionsWithNullLoadBalancer()
    {
        Assert.Throws<ArgumentNullException>(() => new SessionsFactory(null).createSessions(configFileNames));
    }

    public void Dispose(){}
}