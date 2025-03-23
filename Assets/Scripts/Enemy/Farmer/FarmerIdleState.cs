using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerIdleState : IState
{
    private FarmerFSM fsm;
    Coroutine patrol;
    public FarmerIdleState(FarmerFSM fsm)
    {
        this.fsm = fsm;
    }

    public void OnEnter()
    {
        fsm.OnEnter(FarmerStateType.Idle);
        patrol = fsm.StartCoroutine(Patrol());
    }

    public void OnExit()
    {
        if (patrol != null)
            fsm.StopCoroutine(Patrol());
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
        if (fsm.IsDetectObjectByLayer(fsm.param.attackDetectRange, LayerMask.GetMask("Player", "Bubble"), out var _, (int)fsm.transform.localScale.x))
            fsm.ChangeState(FarmerStateType.Chase);
    }

    IEnumerator Patrol()
    {
        yield return new WaitForSeconds(Random.Range(fsm.param.idleToPatrolTime.x, fsm.param.idleToPatrolTime.y));
        fsm.ChangeState(FarmerStateType.Patrol);
    }
}
