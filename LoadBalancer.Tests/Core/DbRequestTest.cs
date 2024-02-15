using System;
using JetBrains.Annotations;
using LoadBalancer.Core;
using Xunit;

namespace LoadBalancer.Tests.Core;

public class DbRequestTest : IDisposable
{
    DbRequest dbRequest;
    public DbRequestTest()
    {
        dbRequest = new DbRequest(new object(), DbRequest.Type.SELECT);
    }

    [Fact]
    public void testDbRequest()
    {
        Assert.NotNull(dbRequest);
    }
    [Fact]
    public void testGetObject()
    {
        Assert.NotNull(dbRequest.getObject());
    }
    [Fact]
    public void testGetType()
    {
        Assert.Equal(DbRequest.Type.SELECT, dbRequest.getType());
    }
    [Fact]
    public void testGetTypeFail()
    {
        Assert.NotEqual(DbRequest.Type.INSERT, dbRequest.getType());
    }

    public void Dispose(){}
}