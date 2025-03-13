using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ChickenManSprintAttackState : IState
{
    private ChickenManFSM fSM;
    private ChickenManParameters parameters;

    public ChickenManSprintAttackState(ChickenManFSM fSM)
    {
        this.fSM = fSM;
        this.parameters = fSM.parameters;
    }

    public void OnEnter()
    {
        var tw = fSM.GetOrAddComponent<Tween>();
        var curveTime = parameters.sprintAttackCurve.keys.Last().time;
        tw.AddTween("sprint", x =>
            fSM.rb.linearVelocityX =
                    fSM.attackDirection * parameters.sprintAttackCurve.Evaluate(x)
                    , 0.0f, curveTime, curveTime)
                    .AddTween(_ => fSM.ChangeState(ChickenManStateType.Patrol), 0, 0, 0).Play();

        return;
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
