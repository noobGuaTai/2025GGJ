using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringManIdleState : IState
{
    SpringManFSM fsm;
    Coroutine patrol;

    public SpringManIdleState(SpringManFSM fsm) => this.fsm = fsm;
    

    public void OnEnter()
    {
        fsm.OnEnter(SpringManStateType.Idle);
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
        if (fsm.IsDetectObjectByLayer(fsm.param.attackDetectRange, LayerMask.GetMask("Player"), out var _, (int)fsm.transform.localScale.x))
            fsm.ChangeState(SpringManStateType.Chase);
    }

    IEnumerator Patrol()
    {
        yield return new WaitForSeconds(Random.Range(fsm.param.idleToPatrolTime.x, fsm.param.idleToPatrolTime.y));
        fsm.ChangeState(SpringManStateType.Patrol);
    }
}
