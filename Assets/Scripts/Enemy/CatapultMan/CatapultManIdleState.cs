using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultManIdleState : IState
{
    CatapultManFSM fsm;
    Coroutine patrol;

    public CatapultManIdleState(CatapultManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(CatapultManStateType.Idle);
        patrol = fsm.StartCoroutine(Patrol());
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
            fsm.ChangeState(CatapultManStateType.Chase);
    }

    IEnumerator Patrol()
    {
        yield return new WaitForSeconds(Random.Range(fsm.param.idleToPatrolTime.x, fsm.param.idleToPatrolTime.y));
        fsm.ChangeState(CatapultManStateType.Patrol);
    }
}
