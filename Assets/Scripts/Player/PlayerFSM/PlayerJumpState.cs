using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerJumpState : IState
{
    private PlayerFSM fsm;

    private Vector2 lastInput = Vector2.zero;

    public PlayerJumpState(PlayerFSM playerFSM)
    {
        this.fsm = playerFSM;
    }

    public void OnEnter()
    {
        fsm.param.rb.linearVelocityY = fsm.attributes.jumpSpeed;
        fsm.param.animator.Play("jump_start", 0, 0f);

    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {
        if (!fsm.param.isOnBigBubble)
            fsm.Move();
        if (fsm.param.moveInput.x > 0)
        {
            fsm.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (fsm.param.moveInput.x < 0)
        {
            fsm.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void OnUpdate()
    {
        if (fsm.param.isOnGround)
            fsm.ChangeState(PlayerStateType.Idle);
    }

}