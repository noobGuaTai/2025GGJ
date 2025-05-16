using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBovineManKnockedBackState : IState
{
    NewBovineManFSM fsm;

    public NewBovineManKnockedBackState(NewBovineManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(NewBovineManStateType.KnockedBack);
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
            fsm.ChangeState(NewBovineManStateType.Idle);
        }
    }
}
