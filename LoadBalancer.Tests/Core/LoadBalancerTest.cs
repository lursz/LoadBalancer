using System;
using JetBrains.Annotations;
using LoadBalancer.Abstracts;
using LoadBalancer.Core;
using LoadBalancer.Core.LoadBalanceAlgorithms;
using LoadBalancer.Core.Session;
using LoadBalancer.DataBase.Entities;
using NHibernate;
using Xunit;

namespace LoadBalancer.Tests.Core;

public class LoadBalancerTest : IDisposable
{
    LoadBalancer<DatabaseSession> loadBalancer;
    DatabaseSession[] sessions;
    ILoadBalanceAlgorithm<DatabaseSession> loadBalanceAlgorithm;

    public LoadBalancerTest()
    {
        loadBalanceAlgorithm = new RoundRobin<DatabaseSession>();
        loadBalancer = new LoadBalancer<DatabaseSession>(loadBalanceAlgorithm);

        SessionsFactory sessionsFactory = new SessionsFactory(loadBalancer);

        string[] configFileNames =
        {
            "./Configs/config1.cfg.xml",
            "./Configs/config2.cfg.xml",
            "./Configs/config3.cfg.xml",
        };
        
        sessions = sessionsFactory.createSessions(configFileNames);
        loadBalancer.injectSessions(sessions);
    }
    
    
    [Fact]
    public void testChangeLoadBalanceAlgorithm()
    {
        ILoadBalanceAlgorithm<DatabaseSession> newLoadBalanceAlgorithm = new Random<DatabaseSession>();
        loadBalancer.changeLoadBalanceAlgorithm(newLoadBalanceAlgorithm);
        for (var i = 0; i < sessions.Length; i++)
        {
            var chosenSession = loadBalanceAlgorithm.chooseSession(sessions);
            Assert.Equal(sessions[i % sessions.Length], chosenSession);
        }
    }
    
    [Fact]
    public void testRedirect()
    {
        DbRequest request = new DbRequest(new User(), DbRequest.Type.INSERT);
        loadBalancer.redirect(request);
        foreach (var session in sessions)
        {
            Assert.True(session.isUsed);
        }
    }
    
    [Fact]
    public void testConnection()
    {
        var session = loadBalancer.connection<ISession>();
        Assert.NotNull(session);
    }
    
    public void Dispose() {}
}