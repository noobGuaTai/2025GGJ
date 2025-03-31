using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorerManAttackState : IState
{
    BorerManFSM fsm;
    Collider2D[] collider2D;
    Coroutine attackCoroutine;
    public BorerManAttackState(BorerManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(BorerManStateType.Attack);
        collider2D = fsm.GetComponents<Collider2D>();
        attackCoroutine = fsm.StartCoroutine(Attack());
    }

    public void OnExit()
    {
        if (attackCoroutine != null)
            fsm.StopCoroutine(attackCoroutine);
        attackCoroutine = null;
        Array.ForEach(collider2D, x => x.enabled = true);
    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {

    }

    IEnumerator Attack()
    {
        while (true)
        {
            Array.ForEach(collider2D, x => x.enabled = false);
            yield return new WaitForSeconds(fsm.param.attackTime);

            var hit = Physics2D.Raycast(PlayerFSM.Instance.transform.position, Vector2.down, 100f, LayerMask.GetMask("Ground"));
            fsm.transform.position = hit.collider != null ? hit.point : fsm.transform.position;
            Array.ForEach(collider2D, x => x.enabled = true);
            yield return new WaitForSeconds(fsm.param.attackCoolDown);
        }
    }
}
