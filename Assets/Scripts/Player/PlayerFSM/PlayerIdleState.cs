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
        this.parameters = playerFSM.param;
    }

    public void OnEnter()
    {
        parameters.animator.Play("idle");
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
        if (parameters.jumpInput && parameters.isOnGround)
        {
            playerFSM.ChangeState(PlayerStateType.Jump);
            return;
        }
    }
}
