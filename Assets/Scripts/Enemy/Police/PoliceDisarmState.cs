using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceDisarmState : IState
{
    PoliceFSM fsm;

    public PoliceDisarmState(PoliceFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(PoliceStateType.Disarm);
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
