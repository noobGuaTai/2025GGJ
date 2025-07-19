using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBovineManAttackState : IState
{
    NewBovineManFSM fsm;
    float timer;
    public NewBovineManAttackState(NewBovineManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(NewBovineManStateType.Attack);
        fsm.animator.Play("run", 0, 0);
        timer = 0;
    }

    public void OnExit()
    {
        fsm.CalmDown();
    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;
        if (fsm.IsDetectObjectByLayer(fsm.param.detectRange, LayerMask.GetMask("Player", "Bubble"), out var a, (int)fsm.transform.localScale.x))
            fsm.InertialChaseObject(fsm.param.SprintSpeed, a);
        else if (timer > 0.5f)
            fsm.ChangeState(NewBovineManStateType.Idle);
    }
}
