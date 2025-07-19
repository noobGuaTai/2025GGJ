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
        fSM.animator.Play("run", 0, 0);
    }

    public void OnExit()
    {
        if (patrolCoroutine != null)
            fSM.StopCoroutine(patrolCoroutine);
        patrolCoroutine = null;
    }

    public void OnFixedUpdate()
    {
        // if (fSM.GetComponent<TargetCollect>().attackTarget.Count > 0)
        // {
        //     fSM.ChangeState(FlowerManStateType.Attack);
        // }
    }

    public void OnUpdate()
    {
        StartPatrol();
        if (fSM.IsDetectObjectByLayer(fSM.parameters.attackRange, LayerMask.GetMask("Player"), out var _))
            fSM.ChangeState(FlowerManStateType.Attack);
    }

    void StartPatrol()
    {
        if (patrolCoroutine == null && fSM.parameters.isOnGround)
        {
            fSM.initPos = fSM.transform.position;
            patrolCoroutine = fSM.TwoPointPatrol(new Vector2(fSM.initPos.x + fSM.parameters.patrolParameter.range[0], fSM.initPos.y), new Vector2(fSM.initPos.x + fSM.parameters.patrolParameter.range[1], fSM.initPos.y), fSM.parameters.patrolParameter.speed);
        }
    }
}
