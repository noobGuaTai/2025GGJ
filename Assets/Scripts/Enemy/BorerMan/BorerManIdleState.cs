using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorerManIdleState : IState
{
    BorerManFSM fsm;

    public BorerManIdleState(BorerManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(BorerManStateType.Idle);
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        if (fsm.IsDetectObjectByLayer(fsm.param.attackDetectRange, LayerMask.GetMask("Player"), out var aim))
        {
            fsm.ChangeState(BorerManStateType.Attack);
            fsm.param.aim = aim;
        }

    }
}
