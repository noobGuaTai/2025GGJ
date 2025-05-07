using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultManChaseState : IState
{
    CatapultManFSM fsm;

    public CatapultManChaseState(CatapultManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(CatapultManStateType.Chase);
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        if (fsm.IsDetectObjectByLayer(fsm.param.attackRange, LayerMask.GetMask("Player", "Bubble"), out var aim))
        {
            fsm.ChangeState(CatapultManStateType.Attack);
            fsm.param.attackAim = aim;
        }

        if (!fsm.IsDetectObjectByLayer(fsm.param.detectRange, LayerMask.GetMask("Player", "Bubble"), out var _))
            fsm.ChangeState(CatapultManStateType.Idle);


        if (fsm.IsDetectObjectByLayer(fsm.param.attackDetectRange, LayerMask.GetMask("Bubble"), out var g))
        {
            fsm.ChaseObject(fsm.param.chaseSpeed, g);
            return;
        }
        if (fsm.IsDetectObjectByLayer(fsm.param.attackDetectRange, LayerMask.GetMask("Player"), out var p))
        {
            fsm.ChaseObject(fsm.param.chaseSpeed, p);
            return;
        }
    }
}
