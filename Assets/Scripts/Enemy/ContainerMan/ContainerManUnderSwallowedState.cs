using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerManUnderSwallowedState : IState
{
    ContainerManFSM fsm;

    public ContainerManUnderSwallowedState(ContainerManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(ContainerManStateType.UnderSwallowed);
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
