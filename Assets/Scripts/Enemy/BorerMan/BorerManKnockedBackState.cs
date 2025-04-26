using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorerManKnockedBackState : IState
{
    BorerManFSM fsm;

    public BorerManKnockedBackState(BorerManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(BorerManStateType.KnockedBack);
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
            fsm.ChangeState(BorerManStateType.Idle);
        }
    }
}
