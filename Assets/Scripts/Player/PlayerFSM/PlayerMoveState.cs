using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMoveState : IState
{
    private PlayerFSM fsm;

    private Vector2 lastInput = Vector2.zero;

    public PlayerMoveState(PlayerFSM playerFSM)
    {
        this.fsm = playerFSM;
    }

    public void OnEnter()
    {
        fsm.param.animator.Play("run", 0, 0f);
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {
        fsm.Move();
        // fsm.param.rb.linearVelocity = new Vector2(fsm.param.moveInput.x * fsm.attributes.moveSpeed, fsm.param.rb.linearVelocity.y);
        if (fsm.param.moveInput.x > 0)
        {
            fsm.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (fsm.param.moveInput.x < 0)
        {
            fsm.transform.localScale = new Vector3(1, 1, 1);
        }

        lastInput = fsm.param.moveInput;
        if (fsm.param.jumpInput && (fsm.param.isOnGround || fsm.param.isOnBigBubble))
        {
            fsm.ChangeState(PlayerStateType.Jump);
            return;
        }
        if (fsm.param.moveInput.x == 0)
        {
            fsm.ChangeState(PlayerStateType.Idle);
            return;
        }
    }

    public void OnUpdate()
    {

    }

}