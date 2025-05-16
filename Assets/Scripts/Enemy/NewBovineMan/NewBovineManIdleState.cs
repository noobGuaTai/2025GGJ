using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class NewBovineManIdleState : IState
{
    NewBovineManFSM fsm;
    float timer;
    public NewBovineManIdleState(NewBovineManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(NewBovineManStateType.Idle);
        timer = 0;
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        if (fsm.param.isOnGround && Mathf.Abs(fsm.transform.position.y - fsm.initPos.y) > 5f)
        {
            fsm.initPos = fsm.transform.position;
        }
        timer += Time.deltaTime;
        if (timer < fsm.param.calmDownTime)
            return;
        if (fsm.IsDetectObjectByLayer(0, fsm.param.detectRange, LayerMask.GetMask("Player", "Bubble"), out var a))
            fsm.ChangeState(NewBovineManStateType.Charge, () => fsm.transform.SetScaleX(Mathf.Sign(a.transform.position.x - fsm.transform.position.x)));
    }
}
