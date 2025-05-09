using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ChickenManAttackState : IState
{
    private ChickenManFSM fsm;
    private ChickenManParameters parameters;

    public ChickenManAttackState(ChickenManFSM fSM)
    {
        this.fsm = fSM;
        this.parameters = fSM.parameters;
    }
    public void OnEnter()
    {
        fsm.animator.Play("attack", 0, 0);
        fsm.attackAudio.Play();
        var target = fsm.targetCollect.attackTarget.First();
        var dir = Mathf.Sign(target.transform.position.x - fsm.transform.position.x);
        fsm.attackDirection = dir;
        var tw = fsm.GetOrAddComponent<Tween>();
        var trname = "Attack";
        tw.AddTween(trname, x => fsm.GetComponent<SpriteRenderer>().color = Color.red, 0, 0, 0)
            .AddTween(_ => { }, 0, 0, 0.5f)
            .AddTween(_ => fsm.GetComponent<SpriteRenderer>().color = Color.white, 0, 0, 0)
            .AddTween(_ => fsm.ChangeState(ChickenManStateType.SprintAttack), 0, 0, 0).Play();
    }

    public void OnExit()
    {
        var tw = fsm.GetComponent<Tween>();
        tw.Clear("Attack");
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
    }
}
