using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceAttackState : IState
{
    PoliceFSM fsm;
    Coroutine wait;
    Coroutine disableAttackCollider;
    public PoliceAttackState(PoliceFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(PoliceStateType.Attack);
        fsm.rb.linearVelocity = Vector2.zero;
        if (fsm.param.attackCollider.enabled == false) fsm.EnableAttackCollider();
        wait = fsm.StartCoroutine(Wait());
        disableAttackCollider = fsm.StartCoroutine(DisableAttackCollider());
    }

    public void OnExit()
    {
        if (wait != null)
            fsm.StopCoroutine(wait);
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        fsm.ChangeState(PoliceStateType.Idle);
    }

    IEnumerator DisableAttackCollider()
    {
        yield return new WaitForSeconds(1f);
        fsm.DisableAttackCollider();
    }
}
