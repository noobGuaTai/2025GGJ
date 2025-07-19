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
        fsm.StartCoroutine(SlowDown());
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

    IEnumerator SlowDown()
    {
        float dragFactor = 0.98f;

        while (fsm.param.rb.linearVelocity.magnitude > 40f)
        {
            fsm.param.rb.linearVelocity *= dragFactor;
            yield return new WaitForFixedUpdate();
        }
    }
}
