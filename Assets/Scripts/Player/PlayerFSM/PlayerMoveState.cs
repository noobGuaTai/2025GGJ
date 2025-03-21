using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMoveState : IState
{
    private PlayerFSM fSM;

    private Vector2 lastInput = Vector2.zero;

    public PlayerMoveState(PlayerFSM playerFSM)
    {
        this.fSM = playerFSM;
    }

    public void OnEnter()
    {
        fSM.param.animator.Play("run", 0, 0f);
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {
        fSM.param.rb.linearVelocity = new Vector2(fSM.param.moveInput.x * fSM.attributes.moveSpeed, fSM.param.rb.linearVelocity.y);
        if (fSM.param.moveInput.x > 0)
        {
            fSM.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (fSM.param.moveInput.x < 0)
        {
            fSM.transform.localScale = new Vector3(1, 1, 1);
        }

        lastInput = fSM.param.moveInput;
        if (fSM.param.jumpInput && (fSM.param.isOnGround || fSM.param.isOnBigBubble))
        {
            fSM.ChangeState(PlayerStateType.Jump);
            return;
        }
        if (fSM.param.moveInput.x == 0)
        {
            fSM.ChangeState(PlayerStateType.Idle);
            return;
        }
    }

    public void OnUpdate()
    {

    }

}