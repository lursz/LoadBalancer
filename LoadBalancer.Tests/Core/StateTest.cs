using System;
using JetBrains.Annotations;
using LoadBalancer.Core;
using Xunit;

namespace LoadBalancer.Tests.Core;

public class StateTest : IDisposable
{
    private State _state;

    public StateTest()
    {
        _state = new State(new Up());
    }

    [Fact]
    public void status_ReturnsUp()
    {
        Assert.Equal(Status.UP, _state.status());
    }

    [Fact]
    public void nextState_ReturnsDown()
    {
        _state.nextState();
        Assert.Equal(Status.DOWN, _state.status());
    }

    public void Dispose()
    {
        _state = null;
    }
}


public class UpTest
{

    [Fact]
    public void status_ReturnsUp()
    {
        Up up = new();
        Assert.Equal(Status.UP, up.status());
    }
    [Fact]
    public void nextState_ReturnsDown()
    {
        Up up = new();
        Assert.Equal(Status.DOWN, up.nextState().status());
    }
}

public class DownTest
{
    [Fact]
    public void status_ReturnsDown()
    {
        Down down = new();
        Assert.Equal(Status.DOWN, down.status());
    }
    [Fact]
    public void nextState_ReturnsSync()
    {
        Down down = new();
        Assert.Equal(Status.SYNC, down.nextState().status());
    }
}

public class SyncTest
{
    [Fact]
    public void status_ReturnsSync()
    {
        Sync sync = new();
        Assert.Equal(Status.SYNC, sync.status());
    }
    [Fact]
    public void nextState_ReturnsUp()
    {
        Sync sync = new();
        Assert.Equal(Status.UP, sync.nextState().status());
    }
}


