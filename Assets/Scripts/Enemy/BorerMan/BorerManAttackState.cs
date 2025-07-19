using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorerManAttackState : IState
{
    BorerManFSM fsm;
    Collider2D[] collider2D;
    Coroutine attackCoroutine;
    Tween tween;
    public BorerManAttackState(BorerManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(BorerManStateType.Attack);
        collider2D = fsm.GetComponents<Collider2D>();
        tween = fsm.gameObject.AddComponent<Tween>();
        Attack();
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

    // IEnumerator Attack()
    // {
    //     while (true)
    //     {
    //         Array.ForEach(collider2D, x => x.enabled = false);
    //         fsm.param.drillInAudio.Play();
    //         yield return new WaitForSeconds(fsm.param.attackTime);
    //         // var c = fsm.param.sr.color;
    //         // c = new Color(c.r, c.g, c.b, 1);
    //         fsm.param.drillOutAudio.Play();
    //         var hit = Physics2D.Raycast(fsm.param.aim.transform.position, Vector2.down, 400f, LayerMask.GetMask("Ground"));
    //         fsm.transform.SetPosX(hit.point.x);
    //         fsm.transform.rotation = Quaternion.Euler(0, 0, 180);
    //         var tween = fsm.gameObject.AddComponent<Tween>();
    //         tween.AddTween("Attack", x =>
    //         {
    //             fsm.transform.SetPosY(x);
    //         }, fsm.transform.position.y, fsm.param.aim.transform.position.y + 10f, 0.5f);
    //         tween.AddTween("Attack", x =>
    //         {
    //             fsm.transform.rotation = Quaternion.Euler(0, 0, x);
    //         }, 180, 360, 0.5f).Play();
    //         Array.ForEach(collider2D, x => x.enabled = true);
    //         yield return new WaitForSeconds(fsm.param.attackCoolDown);
    //         if ((fsm.param.aim.transform.position - fsm.transform.position).magnitude > fsm.param.detectRange)
    //         {
    //             fsm.ChangeState(BorerManStateType.Idle);
    //             yield break;
    //         }

    //     }
    // }
    void Attack()
    {

        Array.ForEach(collider2D, x => x.enabled = false);
        fsm.param.drillInAudio.Play();
        tween.AddTween("Attack", x =>
        {
            fsm.transform.SetPosY(x);
        }, fsm.transform.position.y, fsm.transform.position.y - 27f, 1.2f);
        tween.AddTween("Attack", _ =>
        {
            fsm.param.sr.color = new Color(1, 1, 1, 0);
        }, 0, 0, 0f);
        var hit = Physics2D.Raycast(fsm.param.aim.transform.position, Vector2.down, 400f, LayerMask.GetMask("Ground"));
        tween.AddTween("Attack", _ =>
        {
            fsm.transform.SetPosX(hit.point.x);
            Array.ForEach(collider2D, x => x.enabled = true);
            fsm.param.drillOutAudio.Play();
            fsm.transform.rotation = Quaternion.Euler(0, 0, 180);
            fsm.param.sr.color = new Color(1, 1, 1, 1);
        }, 0, 0, 0f);
        tween.AddTween("Attack", x =>
        {
            fsm.transform.SetPosY(x);
        }, fsm.transform.position.y, fsm.param.aim.transform.position.y + 10f, 0.5f);
        tween.AddTween("Attack", x =>
        {
            fsm.transform.rotation = Quaternion.Euler(0, 0, x);
        }, 180, 360, 0.5f);
        tween.AddTween("Attack", _ =>
        {

        }, 0, 0, fsm.param.attackCoolDown);
        tween.AddTween("Attack", _ =>
        {
            fsm.ChangeState(BorerManStateType.Idle);
            tween.Clear();
        }, 0, 0, 0).Play();


    }
}
