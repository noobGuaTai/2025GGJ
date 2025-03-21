using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenManPatrolState : IState
{
    private ChickenManFSM fSM;
    private ChickenManParameters parameters;

    public ChickenManPatrolState(ChickenManFSM fSM)
    {
        this.fSM = fSM;
        this.parameters = fSM.parameters;
    }

    Coroutine patrolCoroutine;
    public void OnEnter()
    {
        patrolCoroutine = fSM.TwoPointPatrol(
            new Vector2(fSM.initPos.x + parameters.patrolParameter.range[0], fSM.initPos.y), new Vector2(fSM.initPos.x + parameters.patrolParameter.range[1], fSM.initPos.y),
            parameters.patrolParameter.speed, isChangeScale:false);
    }

    public void OnExit()
    {
        fSM.StopCoroutine(patrolCoroutine);
    }

    public void OnFixedUpdate()
    {
        if (fSM.GetComponent<TargetCollect>().attackTarget.Count == 0)
            return;
        fSM.ChangeState(ChickenManStateType.Attack);
    }


    public void OnUpdate()
    {
    }
}
