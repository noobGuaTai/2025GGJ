using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerManPatrolState : IState
{
    private FlowerManFSM fSM;

    public FlowerManPatrolState(FlowerManFSM fSM)
    {
        this.fSM = fSM;
    }

    public Coroutine patrolCoroutine;
    public void OnEnter()
    {
        patrolCoroutine = fSM.TwoPointPatrol(
            new Vector2(
                fSM.initPos.x + fSM.parameters.patrolParameter.range[0],
                fSM.initPos.y),
            new Vector2(fSM.initPos.x + fSM.parameters.patrolParameter.range[1],
                fSM.initPos.y),
                fSM.parameters.patrolParameter.speed);
    }

    public void OnExit()
    {
        fSM.StopCoroutine(patrolCoroutine);
    }

    public void OnFixedUpdate()
    {
        if (fSM.GetComponent<TargetCollect>().attackTarget.Count > 0)
        {
            fSM.ChangeState(FlowerManStateType.Attack);
        }
    }

    public void OnUpdate()
    {
    }
}
