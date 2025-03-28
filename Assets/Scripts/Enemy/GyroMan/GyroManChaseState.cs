using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManChaseState : IState
{
    GyroManFSM fsm;

    public GyroManChaseState(GyroManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(GyroManStateType.Chase);
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
            fsm.ChangeState(GyroManStateType.Return);

        if (fsm.IsDetectObjectByLayer(fsm.param.detectRange, LayerMask.GetMask("Bbble"), out var b))
        {
            fsm.InertialChaseObject(fsm.param.chaseSpeed, b);
            return;
        }
        if (fsm.IsDetectObjectByLayer(fsm.param.detectRange, LayerMask.GetMask("Player"), out var p))
        {
            fsm.InertialChaseObject(fsm.param.chaseSpeed, p);
            return;
        }
    }
}
