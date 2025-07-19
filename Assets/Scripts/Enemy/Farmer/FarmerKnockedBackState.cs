using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerKnockedBackState : IState
{
    private FarmerFSM fsm;

    public FarmerKnockedBackState(FarmerFSM fsm)
    {
        this.fsm = fsm;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
        if (fsm.rb.linearVelocity.magnitude < 40f)
        {
            fsm.ChangeState(FarmerStateType.Idle);
        }
    }
}
