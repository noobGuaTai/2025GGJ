using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManIdleState : IState
{
    GyroManFSM fsm;

    public GyroManIdleState(GyroManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(GyroManStateType.Idle);
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        if (fsm.IsDetectObjectByLayer(fsm.param.detectRange, LayerMask.GetMask("Player", "Bubble"), out var _))
            fsm.ChangeState(GyroManStateType.Chase);
    }
}
