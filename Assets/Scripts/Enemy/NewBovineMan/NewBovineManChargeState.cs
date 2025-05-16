using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBovineManChargeState : IState
{
    NewBovineManFSM fsm;
    public NewBovineManChargeState(NewBovineManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(NewBovineManStateType.Charge);
        var sr = fsm.GetComponent<SpriteRenderer>();
        fsm.param.tween.AddTween("Charge", x => sr.color =
        new Color(sr.color.r, x, x), 1, 0f, fsm.param.ChargeTime).Play();
        fsm.Invoke("ToAttack", fsm.param.ChargeTime);
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
