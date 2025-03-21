using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerAttackState : IState
{
    private WorkerFSM fsm;
    Coroutine wait;

    public WorkerAttackState(WorkerFSM fsm)
    {
        this.fsm = fsm;
    }

    public void OnEnter()
    {
        fsm.rb.linearVelocity = Vector2.zero;
        fsm.EnableAttackCollider();
        wait = fsm.StartCoroutine(Wait());
    }

    public void OnExit()
    {
        if (wait != null)
            fsm.StopCoroutine(wait);
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        fsm.DisableAttackCollider();
        fsm.ChangeState(WorkerStateType.Idle);
    }
}
