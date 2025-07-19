using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : IState
{
    private PlayerFSM fsm;

    public PlayerIdleState(PlayerFSM playerFSM)
    {
        this.fsm = playerFSM;
    }

    public void OnEnter()
    {
        fsm.param.animator.Play("idle", 0, 0f);
        fsm.param.rb.linearVelocityX = 0;
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        if (fsm.param.moveInput.x != 0)
        {
            fsm.ChangeState(PlayerStateType.Move);

            return;
        }
        if (fsm.param.jumpInput && (fsm.param.isOnGround || fsm.param.isOnBigBubble))
        {
            fsm.ChangeState(PlayerStateType.Jump);
            return;
        }
    }
}
