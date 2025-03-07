using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomManPatrolState : IState
{
    private MushroomManFSM fSM;
    private MushroomManParameters parameters;

    public MushroomManPatrolState(MushroomManFSM fSM)
    {
        this.fSM = fSM;
        this.parameters = fSM.parameters;
    }

    public void OnEnter()
    {
        fSM.TwoPointPatrol(parameters.patrolPoint[0], parameters.patrolPoint[1], parameters.patrolSpeed);
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {

    }
}
