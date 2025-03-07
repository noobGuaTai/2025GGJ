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

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
        if (fSM.attackTarget.Count == 0)
            return;
        fSM.ChangeState(ChickenManStateType.Attack);
    }


    public void OnUpdate()
    {
    }
}
