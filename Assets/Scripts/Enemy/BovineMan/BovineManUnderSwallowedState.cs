using UnityEngine;

public class BovineManUnderSwallowedState : BovineBaseState
{
    public BovineManUnderSwallowedState(BovineManFSM fSM) : base(fSM)
    {
    }

    public override void OnEnter()
    {
        param.chaseSpeed = 0;
    }

    public override void OnExit()
    {
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }
}