using JetBrains.Annotations;
using LoadBalancer.Core.LoadBalanceAlgorithms;
using LoadBalancer.
using Xunit;

namespace LoadBalancer.Tests.Core.LoadBalanceAlgorithms;

// [TestSubject(typeof(RoundRobin))]
public class RoundRobinTest
{
    [BeforeEach]
    public void Setup()
    {
        Abstracts.ILoadBalanceAlgorithm<DatabaseSession> loadBalanceAlgorithm = new Core.LoadBalanceAlgorithms.RoundRobin<DatabaseSession>();
    }
        
    
    [Fact]
    public void TestChooseSession()
    {
        
        
    }
}