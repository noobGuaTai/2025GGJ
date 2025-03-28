using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringManChaseState : IState
{
    SpringManFSM fsm;

    public SpringManChaseState(SpringManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(SpringManStateType.Chase);
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        if (!fsm.IsDetectObjectByLayer(fsm.param.detectRange, LayerMask.GetMask("Player", "Bubble"), out var _))
            fsm.ChangeState(SpringManStateType.Idle);
        if (fsm.IsDetectObjectByLayer(fsm.param.attackDetectRange, LayerMask.GetMask("Player"), out var p))
        {
            fsm.ChaseObject(fsm.param.chaseSpeed, p);
            return;
        }
    }
}
