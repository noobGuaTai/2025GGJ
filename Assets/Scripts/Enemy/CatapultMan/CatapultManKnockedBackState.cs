using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultManKnockedBackState : IState
{
    CatapultManFSM fsm;

    public CatapultManKnockedBackState(CatapultManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(CatapultManStateType.KnockedBack);
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
