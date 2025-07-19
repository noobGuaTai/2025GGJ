using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerKnockedBackState : IState
{
    private WorkerFSM fsm;

    public WorkerKnockedBackState(WorkerFSM fsm)
    {
        this.fsm = fsm;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {
        fsm.DisableAttackCollider();
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
        if (fsm.rb.linearVelocity.magnitude < 40f)
        {
            fsm.ChangeState(WorkerStateType.Idle);
        }
    }
}
