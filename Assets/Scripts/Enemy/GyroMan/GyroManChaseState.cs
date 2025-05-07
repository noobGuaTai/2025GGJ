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
        if (fsm.IsDetectObjectByLayer(fsm.param.detectRange, LayerMask.GetMask("Bubble"), out var b))
        {
            fsm.InertialChaseObject(fsm.param.chaseSpeed, b);
            return;
        }
        if (fsm.IsDetectObjectByLayer(fsm.param.detectRange, LayerMask.GetMask("Player"), out var p))
        {
            fsm.InertialChaseObject(fsm.param.chaseSpeed, p);
            return;
        }
        fsm.rb.AddForce(new Vector2(-fsm.param.decelerateSpeed * Mathf.Sign(fsm.rb.linearVelocityX), 0), ForceMode2D.Impulse);
        if (Mathf.Abs(fsm.rb.linearVelocityX) < 0.1f)
            fsm.ChangeState(GyroManStateType.Return);

    }

    IEnumerator Chase()
    {

        yield return null;
    }
}
