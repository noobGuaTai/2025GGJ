using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockedBackState : IState
{
    private PlayerFSM fsm;

    public PlayerKnockedBackState(PlayerFSM fsm)
    {
        this.fsm = fsm;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
        if (fsm.param.rb.linearVelocity.magnitude < 40f)
        {
            fsm.ChangeState(PlayerStateType.Idle);
        }
    }
}
