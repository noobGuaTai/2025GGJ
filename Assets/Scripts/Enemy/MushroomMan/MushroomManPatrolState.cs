using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomManPatrolState : IState
{
    private MushroomManFSM fSM;
    private MushroomManParameters parameters;
    Coroutine patrolCoroutine;

    public MushroomManPatrolState(MushroomManFSM fSM)
    {
        this.fSM = fSM;
        this.parameters = fSM.parameters;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {
        if (patrolCoroutine != null)
            fSM.StopCoroutine(patrolCoroutine);
        patrolCoroutine = null;
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
        StartPatrol();
        if (fSM.IsDetectObjectByLayer(parameters.attackRange, LayerMask.GetMask("Player"), out var _))
            fSM.ChangeState(MushroomManStateType.Attack);
    }

    void StartPatrol()
    {
        if (patrolCoroutine == null && fSM.parameters.isOnGround)
        {
            fSM.initPos = fSM.transform.position;
            patrolCoroutine = fSM.TwoPointPatrol(new Vector2(fSM.initPos.x + parameters.patrolPoint[0], fSM.initPos.y), new Vector2(fSM.initPos.x + parameters.patrolPoint[1], fSM.initPos.y), parameters.patrolSpeed);
        }
    }
}
