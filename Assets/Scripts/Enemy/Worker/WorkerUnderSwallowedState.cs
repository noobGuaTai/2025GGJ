using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerUnderSwallowedState : IState
{
    private WorkerFSM fsm;

    public WorkerUnderSwallowedState(WorkerFSM fsm)
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
    }
}
