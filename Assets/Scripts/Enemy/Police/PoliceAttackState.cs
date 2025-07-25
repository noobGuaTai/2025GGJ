using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        fsm.animator.Play("attack", 0, 0);
        fsm.rb.linearVelocity = Vector2.zero;
        fsm.transform.localScale = new Vector3(fsm.transform.position.x < PlayerFSM.Instance.transform.position.x ? 1 : -1, 1, 1);
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
        yield return new WaitForSeconds(0.625f);
        fsm.ChangeState(PoliceStateType.Idle);
    }

    IEnumerator DisableAttackCollider()
    {
        yield return new WaitForSeconds(0.6f);
        if (fsm.param.attackCollider.All(x => x.enabled == false)) fsm.EnableAttackCollider();
        fsm.attackAudio.Play();
        yield return new WaitForSeconds(0.1f);
        fsm.DisableAttackCollider();
    }
}
