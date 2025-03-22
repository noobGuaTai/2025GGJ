using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerPatrolState : IState
{
    private WorkerFSM fsm;
    Coroutine patrolCoroutine;
    Coroutine relax;

    public WorkerPatrolState(WorkerFSM fsm)
    {
        this.fsm = fsm;
    }

    public void OnEnter()
    {
        relax = fsm.StartCoroutine(Relax());
    }

    public void OnExit()
    {
        if (patrolCoroutine != null)
            fsm.StopCoroutine(patrolCoroutine);
        patrolCoroutine = null;
        if (relax != null)
            fsm.StopCoroutine(relax);
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
        StartPatrol();
        if (fsm.IsDetectObjectByLayer(fsm.param.attackDetectRange, LayerMask.GetMask("Player", "Bubble"), out var _))
            fsm.ChangeState(WorkerStateType.Chase);
    }

    void StartPatrol()
    {
        if (patrolCoroutine == null && fsm.param.isOnGround)
        {
            fsm.initPos = fsm.transform.position;
            patrolCoroutine = fsm.RandomRangePatrol(
            new Vector2(fsm.initPos.x + fsm.param.patrolPoint[0], fsm.initPos.y),
            new Vector2(fsm.initPos.x + fsm.param.patrolPoint[1], fsm.initPos.y),
            fsm.param.patrolSpeed, fsm.param.patrolMinDistance);
        }
    }

    IEnumerator Relax()
    {
        yield return new WaitForSeconds(Random.Range(fsm.param.patrolToIdleTime.x, fsm.param.patrolToIdleTime.y));
        fsm.ChangeState(WorkerStateType.Idle);
    }
}
