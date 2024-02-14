using System;
using LoadBalancer.Abstracts;
using LoadBalancer.Core;
using LoadBalancer.Core.LoadBalanceAlgorithms;
using LoadBalancer.Core.Session;
using Xunit;

namespace LoadBalancer.Tests.Core.LoadBalanceAlgorithms;

public class RandomTest : IDisposable
{
    ILoadBalanceAlgorithm<DatabaseSession> loadBalanceAlgorithm = new Random<DatabaseSession>();
    DatabaseSession[] sessions;
    
    public RandomTest()
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
        loadBalancer.injectSessions(sessions);

    }

    [Fact]
    public void testChooseSession()
    {
                
        var chosenSession = loadBalanceAlgorithm.chooseSession(sessions);
        Assert.NotNull(chosenSession);
        
    }

    public void Dispose(){}
}