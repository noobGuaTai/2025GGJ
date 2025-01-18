using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMoveState : IState
{
    private PlayerFSM playerFSM;
    private PlayerParameters parameters;

    private Vector2 lastInput = Vector2.zero;

    public PlayerMoveState(PlayerFSM playerFSM)
    {
        this.playerFSM = playerFSM;
        this.parameters = playerFSM.parameters;
    }

    public void OnEnter()
    {
        Debug.Log("Move");
        parameters.animator.Play("run");
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {
        parameters.rb.linearVelocity = new Vector2(parameters.moveInput.x * parameters.moveSpeed, parameters.rb.linearVelocity.y);
        // if (parameters.moveInput.x > 0)
        // {
        //     // playerFSM.parameters.sr.sprite = parameters.walkSprites[0];
        //     // parameters.animator.Play("WalkRight");
        // }
        // // else if (parameters.moveInput.x < 0)
        // // {
        //     // playerFSM.parameters.sr.sprite = parameters.walkSprites[1];
        //     // parameters.animator.Play("WalkLeft");
        // // }
        // // else if (parameters.moveInput.y < 0)
        // // {
        // //     playerFSM.parameters.sr.sprite = parameters.walkSprites[3];
        // //     parameters.animator.Play("WalkDown");
        // // }
        if (parameters.moveInput.x > 0)
        {
            playerFSM.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (parameters.moveInput.x < 0)
        {
            playerFSM.transform.localScale = new Vector3(1, 1, 1);
        }

        lastInput = parameters.moveInput;
        if (parameters.moveInput.y > 0)
        {
            playerFSM.ChangeState(PlayerStateType.Jump);
            return;
        }
        if (parameters.moveInput == Vector2.zero)
        {
            playerFSM.ChangeState(PlayerStateType.Idle);
            return;
        }
    }

    public void OnUpdate()
    {

    }

}