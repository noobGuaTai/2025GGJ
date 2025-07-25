using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerIdleState : IState
{
    private WorkerFSM fsm;
    Coroutine patrol;

    public WorkerIdleState(WorkerFSM fsm)
    {
        this.fsm = fsm;
    }

    public void OnEnter()
    {
        patrol = fsm.StartCoroutine(Patrol());
        fsm.animator.Play("idle", 0, 0);
    }

    public void OnExit()
    {
        if (patrol != null)
            fsm.StopCoroutine(patrol);
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
        if (fsm.IsDetectObjectByLayer(fsm.param.attackDetectRange, LayerMask.GetMask("Player", "Bubble"), out var _, (int)fsm.transform.localScale.x))
            fsm.ChangeState(WorkerStateType.Chase);
    }

    IEnumerator Patrol()
    {
        yield return new WaitForSeconds(Random.Range(fsm.param.idleToPatrolTime.x, fsm.param.idleToPatrolTime.y));
        fsm.ChangeState(WorkerStateType.Patrol);
    }
}
