using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RichmanIdleState : IState
{
    RichmanFSM fsm;

    public RichmanIdleState(RichmanFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(RichmanStateType.Idle);
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
