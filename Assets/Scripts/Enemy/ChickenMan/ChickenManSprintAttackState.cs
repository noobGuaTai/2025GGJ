using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ChickenManSprintAttackState : IState
{
    private ChickenManFSM fsm;
    private ChickenManParameters parameters;

    public ChickenManSprintAttackState(ChickenManFSM fSM)
    {
        this.fsm = fSM;
        this.parameters = fSM.parameters;
    }

    public void OnEnter()
    {
        var tw = fsm.GetOrAddComponent<Tween>();
        var curveTime = parameters.sprintAttackCurve.keys.Last().time;
        tw.AddTween("sprint", x =>
            fsm.rb.linearVelocityX =
                    fsm.attackDirection * parameters.sprintAttackCurve.Evaluate(x)
                    , 0.0f, curveTime, curveTime)
                    .AddTween(_ => fsm.ChangeState(ChickenManStateType.Patrol), 0, 0, 0).Play();

        return;
    }

    public void OnExit()
    {
        var tw = fsm.GetComponent<Tween>();
        tw.Clear("sprint");
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
    }
}
