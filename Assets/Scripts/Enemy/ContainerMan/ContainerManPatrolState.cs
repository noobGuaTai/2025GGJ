using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerManPatrolState : IState
{
    ContainerManFSM fsm;
    Coroutine patrolCoroutine;
    Coroutine relax;

    public ContainerManPatrolState(ContainerManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(ContainerManStateType.Patrol);
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
            fsm.ChangeState(ContainerManStateType.Chase);
    }

    void StartPatrol()
    {
        if (patrolCoroutine == null && fsm.param.isOnGround)
        {
            fsm.initPos = fsm.transform.position;
            patrolCoroutine = fsm.TwoPointPatrol(
            new Vector2(fsm.initPos.x + fsm.param.patrolPoint[0], fsm.initPos.y),
            new Vector2(fsm.initPos.x + fsm.param.patrolPoint[1], fsm.initPos.y),
            fsm.param.patrolSpeed);
        }
    }

    IEnumerator Relax()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(fsm.param.patrolToIdleTime.x, fsm.param.patrolToIdleTime.y));
        fsm.ChangeState(ContainerManStateType.Idle);
    }
}
