using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerChaseState : IState
{
    private FarmerFSM fsm;

    public FarmerChaseState(FarmerFSM fsm)
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
        if (fsm.IsDetectObjectByLayer(fsm.param.attackRange, LayerMask.GetMask("Player", "Bubble"), out var _) &&
        !fsm.IsDetectObjectByLayer(fsm.param.pullRange, LayerMask.GetMask("Player"), out var _))
            fsm.ChangeState(FarmerStateType.Attack);
        if (!fsm.IsDetectObjectByLayer(fsm.param.detectRange, LayerMask.GetMask("Player", "Bubble"), out var _))
            fsm.ChangeState(FarmerStateType.Idle);


        if (fsm.IsDetectObjectByLayer(fsm.param.attackDetectRange, LayerMask.GetMask("Bubble"), out var g))
        {
            fsm.ChaseObject(fsm.param.chaseSpeed, g);
            return;
        }
        if (fsm.IsDetectObjectByLayer(fsm.param.attackDetectRange, LayerMask.GetMask("Player"), out var p))
        {
            if (!fsm.IsDetectObjectByLayer(fsm.param.pullRange, LayerMask.GetMask("Player"), out var _))
                fsm.ChaseObject(fsm.param.chaseSpeed, p);
            else
                fsm.ChaseObject(-fsm.param.chaseSpeed, p);
            return;
        }
    }
}
