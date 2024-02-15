namespace LoadBalancer.Core;

public enum Status { UP, DOWN, SYNC }

public class State
{
    private IState _state;

    public State(IState state)
    {
        _state = state;
    }

    public Status status()
    {
        return _state.status();
    }
    public void nextState()
    {
        _state = _state.nextState();
    }
    
}

public class Up : IState
{
    public Status status()
    {
        return Status.UP;
    }
    public IState nextState()
    {
        return new Down();
    }
}

public class Down : IState
{
    public Status status()
    {
        return Status.DOWN;
    }
    public IState nextState()
    {
        return new Sync();
    }
}

public class Sync : IState
{
    public Status status()
    {
        return Status.SYNC;
    }
    public IState nextState()
    {
        return new Up();
    }
}
public interface IState
{
    public Status status();
    public IState nextState();
}


    
    