public class BovineBaseState : IState
{
    protected BovineManFSM fSM;
    protected BovineManParameters parameters;
    public BovineBaseState(BovineManFSM fSM)
    {
        this.fSM = fSM;
        parameters = fSM.parameters;
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