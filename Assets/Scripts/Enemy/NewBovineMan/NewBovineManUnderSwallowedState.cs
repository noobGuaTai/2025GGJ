using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBovineManUnderSwallowedState : IState
{
    NewBovineManFSM fsm;

    public NewBovineManUnderSwallowedState(NewBovineManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(NewBovineManStateType.UnderSwallowed);
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
