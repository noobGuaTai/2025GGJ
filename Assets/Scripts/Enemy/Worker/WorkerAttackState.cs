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
        fsm.animator.Play("attack", 0, 0);
        fsm.rb.linearVelocity = Vector2.zero;
        wait = fsm.StartCoroutine(Wait());
    }

    public void OnExit()
    {
        if (wait != null)
            fsm.StopCoroutine(wait);
        fsm.DisableAttackCollider();
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
    }

    IEnumerator Wait()
    {
        fsm.attackAudio.Play();
        yield return new WaitForSeconds(0.625f);
        fsm.animator.Play("run", 0, 0);
        yield return new WaitForSeconds(0.875f);
        fsm.DisableAttackCollider();
        fsm.ChangeState(WorkerStateType.Idle);
    }
}
