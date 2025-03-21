using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerJumpState : IState
{
    private PlayerFSM fSM;

    private Vector2 lastInput = Vector2.zero;

    public PlayerJumpState(PlayerFSM playerFSM)
    {
        this.fSM = playerFSM;
    }

    public void OnEnter()
    {
        fSM.param.rb.linearVelocityY = fSM.attributes.jumpSpeed;
        fSM.param.animator.Play("jump", 0, 0f);

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
    }

    public void OnUpdate()
    {

    }

}