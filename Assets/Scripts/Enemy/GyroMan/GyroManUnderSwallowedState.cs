using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManUnderSwallowedState : IState
{
    GyroManFSM fsm;

    public GyroManUnderSwallowedState(GyroManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(GyroManStateType.UnderSwallowed);
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
