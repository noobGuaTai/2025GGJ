using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultManUnderSwallowedState : IState
{
    CatapultManFSM fsm;

    public CatapultManUnderSwallowedState(CatapultManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(CatapultManStateType.UnderSwallowed);
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
