using System;
using JetBrains.Annotations;
using LoadBalancer.Core;
using LoadBalancer.Core.LoadBalanceAlgorithms;
using LoadBalancer.Core.Session;
using Xunit;

namespace LoadBalancer.Tests.Core.Session;

public class LoadBalancerInterceptorTest : IDisposable
{
    LoadBalancerInterceptor interceptor;
    public LoadBalancerInterceptorTest()
    {
        LoadBalancer<DatabaseSession> loadBalancer = new LoadBalancer<DatabaseSession>(new RoundRobin<DatabaseSession>());
        interceptor = new LoadBalancerInterceptor(loadBalancer);    
    }

    [Fact]
    public void testOnSave()
    {
        Assert.True(interceptor.OnSave(null, null, null, null, null));
    }
    
    [Fact]
    public void testOnDelete()
    {
        Assert.Throws<System.Exception>(() => interceptor.OnDelete(null, null, null, null, null));
    }
    
    [Fact]
    public void testOnFlushDirty()
    {
        Assert.False(interceptor.OnFlushDirty(null, null, null, null, null, null));
    }
    

    public void Dispose() {}
}