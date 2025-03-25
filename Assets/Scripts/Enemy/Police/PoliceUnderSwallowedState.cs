using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceUnderSwallowedState : IState
{
    PoliceFSM fsm;

    public PoliceUnderSwallowedState(PoliceFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(PoliceStateType.UnderSwallowed);
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
