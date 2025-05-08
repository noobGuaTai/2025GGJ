using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : IState
{
    BossFSM fsm;

    public BossAttackState(BossFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(BossStateType.Attack);
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
