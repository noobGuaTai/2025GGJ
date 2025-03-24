using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceKnockedBackState : IState
{
    PoliceFSM fsm;

    public PoliceKnockedBackState(PoliceFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(PoliceStateType.KnockedBack);
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
            fsm.ChangeState(PoliceStateType.Idle);
        }
    }
}
