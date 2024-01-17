namespace LoadBalancer.Core;

public class State(IState state)
{
    private IState _state = state;
    
    public void nextState()
    {
        _state = _state.nextState();
    }
    
}




class Up : IState
{
    public IState nextState()
    {
        return new Down();
    }
}

class Down : IState
{
    public IState nextState()
    {
        return new Sync();
    }
}

class Sync : IState
{
    public IState nextState()
    {
        return new Up();
    }
}
public interface IState
{
    public IState nextState();
}


    
    