using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BovineManReturnState : BovineBaseState
{
    Coroutine returnCoroutine;

    public BovineManReturnState(BovineManFSM _fsm) : base(_fsm)
    {
    }

    override public void OnEnter()
    {
    }

    override public void OnExit()
    {
        if (returnCoroutine != null)
            fsm.StopCoroutine(returnCoroutine);
        returnCoroutine = null;
    }

    override public void OnFixedUpdate()
    {

    }

    override public void OnUpdate()
    {
        StartReturn();
        if (fsm.IsDetectObjectByLayer(fsm.param.detectRange, LayerMask.GetMask("Player", "Bubble"), out var _))
            fsm.ChangeState(BovineManStateType.ChargedEnergy);
        if ((fsm.transform.position - (Vector3)fsm.initPos).magnitude < 0.1f)
        {
            fsm.ChangeState(BovineManStateType.Patrol);
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
