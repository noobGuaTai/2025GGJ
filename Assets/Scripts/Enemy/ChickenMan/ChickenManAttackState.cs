using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ChickenManAttackState : IState
{
    private ChickenManFSM fSM;
    private ChickenManParameters parameters;

    public ChickenManAttackState(ChickenManFSM fSM)
    {
        this.fSM = fSM;
        this.parameters = fSM.parameters;
    }
    public void OnEnter()
    {
        var target = fSM.GetComponent<TargetCollect>().attackTarget.First();
        var dir = Mathf.Sign(target.transform.position.x - fSM.transform.position.x);
        fSM.attackDirection = dir;
        var tw = fSM.GetOrAddComponent<Tween>();
        var trname = "Attack";
        tw.AddTween(trname, x => fSM.GetComponent<SpriteRenderer>().color = Color.red, 0, 0, 0)
            .AddTween(_ => { }, 0, 0, 0.5f)
            .AddTween(_ => fSM.GetComponent<SpriteRenderer>().color = Color.white, 0,0,0)
            .AddTween(_ => fSM.ChangeState(ChickenManStateType.SprintAttack), 0, 0, 0 ).Play();
        
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
