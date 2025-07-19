using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBovineManReturnState : IState
{
    NewBovineManFSM fsm;

    public NewBovineManReturnState(NewBovineManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(NewBovineManStateType.Return);
        fsm.animator.Play("run", 0, 0);
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        fsm.ChasePosition(fsm.param.returnSpeed, fsm.initPos);
        if (Mathf.Abs(fsm.transform.position.x - fsm.initPos.x) < 1f)
        {
            fsm.ChangeState(NewBovineManStateType.Idle);
        }
    }
}
