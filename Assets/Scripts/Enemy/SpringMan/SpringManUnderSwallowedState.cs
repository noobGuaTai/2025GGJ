using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringManUnderSwallowedState : IState
{
    SpringManFSM fsm;

    public SpringManUnderSwallowedState(SpringManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(SpringManStateType.UnderSwallowed);
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
