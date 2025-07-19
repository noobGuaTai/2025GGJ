using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenManPatrolState : IState
{
    private ChickenManFSM fsm;
    private ChickenManParameters parameters;

    public ChickenManPatrolState(ChickenManFSM fSM)
    {
        this.fsm = fSM;
        this.parameters = fSM.parameters;
    }

    Coroutine patrolCoroutine;
    public void OnEnter()
    {
        fsm.animator.Play("run", 0, 0);
        patrolCoroutine = fsm.TwoPointPatrol(
            new Vector2(fsm.initPos.x + parameters.patrolParameter.range[0], fsm.initPos.y), new Vector2(fsm.initPos.x + parameters.patrolParameter.range[1], fsm.initPos.y),
            parameters.patrolParameter.speed, isChangeScale: false);
    }

    public void OnExit()
    {
        fsm.StopCoroutine(patrolCoroutine);
    }

    public void OnFixedUpdate()
    {
        if (fsm.targetCollect.attackTarget.Count == 0)
            return;
        fsm.ChangeState(ChickenManStateType.Attack);
    }


    public void OnUpdate()
    {
    }
}
