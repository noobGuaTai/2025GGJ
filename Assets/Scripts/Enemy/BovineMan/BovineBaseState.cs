public class BovineBaseState : IState
{
    protected BovineManFSM fsm;
    protected BovineManParameters param;
    public BovineBaseState(BovineManFSM fSM)
    {
        this.fsm = fSM;
        param = fSM.param;
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