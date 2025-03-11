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
        patrolCoroutine = fSM.TwoPointPatrol(new Vector2(fSM.initPos.x + parameters.patrolPoint[0], fSM.initPos.y), new Vector2(fSM.initPos.x + parameters.patrolPoint[1], fSM.initPos.y), parameters.patrolSpeed);
    }

    public void OnExit()
    {
        fSM.StopCoroutine(patrolCoroutine);
        // fSM.StopAllCoroutines();
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
        if (fSM.DetectPlayer(parameters.attackRange))
            fSM.ChangeState(MushroomManStateType.Attack);
    }
}
