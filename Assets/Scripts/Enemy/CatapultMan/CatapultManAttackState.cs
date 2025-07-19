using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultManAttackState : IState
{
    CatapultManFSM fsm;
    Coroutine wait;
    public CatapultManAttackState(CatapultManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(CatapultManStateType.Attack);
        fsm.attackAudio.Play();
        fsm.animator.Play("attack", 0, 0);
        fsm.rb.linearVelocity = Vector2.zero;
        wait = fsm.StartCoroutine(Wait());
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
        yield return new WaitForSeconds(Random.Range(fsm.param.attackCooldown.x, fsm.param.attackCooldown.y));
        fsm.ChangeState(CatapultManStateType.Idle);
    }
}
