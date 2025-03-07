public class BovineBaseState : IState
{
    protected BovineManFSM fsm;
    protected BovineManParameters parameters;
    public BovineBaseState(BovineManFSM _fsm)
    {
        fsm = _fsm;
        parameters = _fsm.parameters;
    }
    public virtual void OnEnter()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual void OnFixedUpdate()
    {
    }

    public virtual void OnUpdate()
    {
    }
}