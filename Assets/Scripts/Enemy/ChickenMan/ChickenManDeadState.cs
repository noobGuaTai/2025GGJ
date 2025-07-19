using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenManDeadState : IState
{
    private ChickenManFSM fsm;
    private ChickenManParameters parameters;

    public ChickenManDeadState(ChickenManFSM fSM)
    {
        this.fsm = fSM;
        this.parameters = fSM.parameters;
    }

    public void OnEnter()
    {
        fsm.animator.Play("die", 0, 0);

    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
    }
}
