using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : IState
{
    private PlayerFSM playerFSM;
    private PlayerParameters parameters;

    public PlayerIdleState(PlayerFSM playerFSM)
    {
        this.playerFSM = playerFSM;
        this.parameters = playerFSM.parameters;
    }

    public void OnEnter()
    {
        parameters.animator.Play("idle");
        Debug.Log("Idle");
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        if (parameters.moveInput.x != 0)
        {
            playerFSM.ChangeState(PlayerStateType.Move);

            return;
        }
        if (parameters.moveInput.y > 0)
        {
            playerFSM.ChangeState(PlayerStateType.Jump);
            return;
        }
    }
}
