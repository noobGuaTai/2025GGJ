using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerJumpState : IState
{
    private PlayerFSM playerFSM;
    private PlayerParameters parameters;

    private Vector2 lastInput = Vector2.zero;

    public PlayerJumpState(PlayerFSM playerFSM)
    {
        this.playerFSM = playerFSM;
        this.parameters = playerFSM.parameters;
    }

    public void OnEnter()
    {
        parameters.rb.AddForce(Vector2.up * parameters.jumpForce);
        parameters.animator.Play("jump");

    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {
        parameters.rb.linearVelocity = new Vector2(parameters.moveInput.x * parameters.moveSpeed, parameters.rb.linearVelocity.y);
        if (parameters.moveInput.x > 0)
        {
            playerFSM.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (parameters.moveInput.x < 0)
        {
            playerFSM.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void OnUpdate()
    {

    }

}