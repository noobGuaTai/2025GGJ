using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringManKnockedBackState : IState
{
    SpringManFSM fsm;

    public SpringManKnockedBackState(SpringManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(SpringManStateType.KnockedBack);
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
            fsm.ChangeState(SpringManStateType.Idle);
        }
    }
}
