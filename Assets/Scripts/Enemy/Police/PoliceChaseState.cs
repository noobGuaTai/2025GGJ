using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceChaseState : IState
{
    PoliceFSM fsm;

    public PoliceChaseState(PoliceFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(PoliceStateType.Chase);
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
            fsm.ChangeState(PoliceStateType.Attack);
        if (!fsm.IsDetectObjectByLayer(fsm.param.detectRange, LayerMask.GetMask("Player", "Bubble"), out var _) &&
        !fsm.IsDetectObjectByComponent<WeaponCoin>(fsm.param.detectRange, out var _, customLayer: LayerMask.GetMask("Item")))
            fsm.ChangeState(PoliceStateType.Idle);


        if (fsm.IsDetectObjectByLayer(fsm.param.attackDetectRange, LayerMask.GetMask("Coin"), out var g))
        {
            fsm.ChaseObject(fsm.param.chaseSpeed, g);
            return;
        }
        if (fsm.IsDetectObjectByLayer(fsm.param.attackDetectRange, LayerMask.GetMask("Bbble"), out var b))
        {
            fsm.ChaseObject(fsm.param.chaseSpeed, b);
            return;
        }
        if (fsm.IsDetectObjectByLayer(fsm.param.attackDetectRange, LayerMask.GetMask("Player"), out var p))
        {
            fsm.ChaseObject(fsm.param.chaseSpeed, p);
            return;
        }
    }
}
