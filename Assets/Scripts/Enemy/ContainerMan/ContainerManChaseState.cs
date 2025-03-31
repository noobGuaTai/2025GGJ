using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerManChaseState : IState
{
    ContainerManFSM fsm;

    public ContainerManChaseState(ContainerManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(ContainerManStateType.Chase);
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
            fsm.ChangeState(ContainerManStateType.Attack);
        if (!fsm.IsDetectObjectByLayer(fsm.param.detectRange, LayerMask.GetMask("Player", "Bubble"), out var _))
            fsm.ChangeState(ContainerManStateType.Idle);


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
