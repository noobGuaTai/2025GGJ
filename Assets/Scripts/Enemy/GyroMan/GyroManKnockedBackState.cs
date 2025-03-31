using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManKnockedBackState : IState
{
    GyroManFSM fsm;

    public GyroManKnockedBackState(GyroManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(GyroManStateType.KnockedBack);
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
