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
        fsm.animator.Play("run", 0, 0);
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {

    }

    float chaseCoodDown = 2f;
    float chaseTimer = 2f;
    public void OnUpdate()
    {
        if (!fsm.IsDetectObjectByLayer(fsm.param.detectRange, LayerMask.GetMask("Player", "Bubble"), out var _))
            fsm.ChangeState(SpringManStateType.Idle);
        chaseTimer += Time.deltaTime;
        if (chaseTimer < chaseCoodDown)
            return;
        chaseTimer -= chaseCoodDown;
        if (fsm.IsDetectObjectByLayer(fsm.param.attackDetectRange, LayerMask.GetMask("Player"), out var p))
        {
            fsm.ChaseObject(fsm.param.chaseSpeed, p);
            return;
        }
    }
}
