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
        
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {
        var tw = fSM.GetOrAddComponent<Tween>();
        var curveTime = parameters.sprintAttackCurve.keys.Last().time;
        tw.AddTween("sprint", x =>
            fSM.rb.MovePosition(new Vector2(
                    fSM.attackDirection * parameters.sprintAttackCurve.Evaluate(x), 0)),
                    0, curveTime, curveTime)
                    .AddTween(_ => fSM.ChangeState(ChickenManStateType.Patrol), 0, 0, 0).Play();
    }

    public void OnUpdate()
    {
    }
}
