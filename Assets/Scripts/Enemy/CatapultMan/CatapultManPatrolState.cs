using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultManPatrolState : IState
{
    CatapultManFSM fsm;
    Coroutine patrolCoroutine;
    Coroutine relax;
    public CatapultManPatrolState(CatapultManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(CatapultManStateType.Patrol);
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
            fsm.ChangeState(CatapultManStateType.Chase);
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
        fsm.ChangeState(CatapultManStateType.Idle);
    }
}
