using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReboundState : IState
{
    private PlayerFSM fsm;

    bool canControl;
    public PlayerReboundState(PlayerFSM fsm)
    {
        this.fsm = fsm;
    }

    public void OnEnter()
    {
        fsm.delegateParam.onRebound?.Invoke();
        fsm.delegateParam.onRebound = null;
        canControl = false;
        fsm.CreateFX(fsm.param.StepOnBubbleFX);
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
        if (MathF.Abs(fsm.param.rb.linearVelocity.magnitude) < 40f)
        {
            canControl = true;
        }
        if (canControl)
        {
            fsm.Move();
        }
        if (fsm.param.isOnGround)
            fsm.ChangeState(PlayerStateType.Idle);
    }
}
