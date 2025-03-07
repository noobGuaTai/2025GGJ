using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomManAttackState : IState
{
    private MushroomManFSM fSM;
    private MushroomManParameters parameters;

    public MushroomManAttackState(MushroomManFSM fSM)
    {
        this.fSM = fSM;
        this.parameters = fSM.parameters;
    }

    public void OnEnter()
    {
        
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
