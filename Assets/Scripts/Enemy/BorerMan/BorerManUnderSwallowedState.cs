using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorerManUnderSwallowedState : IState
{
    BorerManFSM fsm;

    public BorerManUnderSwallowedState(BorerManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(BorerManStateType.UnderSwallowed);
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
