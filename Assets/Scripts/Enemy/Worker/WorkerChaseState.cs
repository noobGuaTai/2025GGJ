using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerChaseState : IState
{
    private WorkerFSM fsm;

    public WorkerChaseState(WorkerFSM fsm)
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
        if (fsm.IsDetectObjectByLayer(fsm.param.attackRange, LayerMask.GetMask("Player", "Bubble"), out var _))
            fsm.ChangeState(WorkerStateType.Attack);
        if (!fsm.IsDetectObjectByLayer(fsm.param.detectRange, LayerMask.GetMask("Player", "Bubble"), out var _))
            fsm.ChangeState(WorkerStateType.Idle);


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
