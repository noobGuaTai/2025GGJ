using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManReturnState : IState
{
    GyroManFSM fsm;
    Coroutine returnCoroutine;

    public GyroManReturnState(GyroManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(GyroManStateType.Return);
    }

    public void OnExit()
    {
        if (returnCoroutine != null)
            fsm.StopCoroutine(returnCoroutine);
        returnCoroutine = null;
    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        StartReturn();
        if (fsm.IsDetectObjectByLayer(fsm.param.detectRange, LayerMask.GetMask("Player", "Bubble"), out var _))
            fsm.ChangeState(GyroManStateType.Chase);
        if ((fsm.transform.position - (Vector3)fsm.initPos).magnitude < 0.1f)
        {
            fsm.ChangeState(GyroManStateType.Idle);
        }
    }

    void StartReturn()
    {
        if (returnCoroutine == null && fsm.param.isOnGround)
        {
            returnCoroutine = fsm.ReturnToInitPos(fsm.param.returnSpeed);
        }
    }
}
