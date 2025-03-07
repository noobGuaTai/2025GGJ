using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChickenManAttackState : IState
{
    private ChickenManFSM fSM;
    private ChickenManParameters parameters;

    public ChickenManAttackState(ChickenManFSM fSM)
    {
        this.fSM = fSM;
        this.parameters = fSM.parameters;
    }
    public void OnEnter()
    {
        var target = fSM.attackTarget.First();
        var dir = Mathf.Sign(target.transform.position.x - fSM.transform.position.x);
        fSM.attackDirection = dir;
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
        fSM.ChangeState(ChickenManStateType.SprintAttack);
    }

    public void OnUpdate()
    {
    }
}
