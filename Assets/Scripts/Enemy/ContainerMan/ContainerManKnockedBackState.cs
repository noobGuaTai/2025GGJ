using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerManKnockedBackState : IState
{
    ContainerManFSM fsm;

    public ContainerManKnockedBackState(ContainerManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(ContainerManStateType.KnockedBack);
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
