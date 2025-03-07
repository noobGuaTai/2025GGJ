using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenManDeadState : IState
{
    private ChickenManFSM fSM;
    private ChickenManParameters parameters;

    public ChickenManDeadState(ChickenManFSM fSM)
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
