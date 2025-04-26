using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerManAttackState : IState
{
    ContainerManFSM fsm;
    Coroutine wait;

    public ContainerManAttackState(ContainerManFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(ContainerManStateType.Attack);
        fsm.animator.Play("attack", 0, 0);
        fsm.rb.linearVelocity = Vector2.zero;
        wait = fsm.StartCoroutine(Wait());
        Attack();


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
        yield return new WaitForSeconds(fsm.param.attackCooldown);
        if (fsm.IsDetectObjectByLayer(fsm.param.attackDetectRange, LayerMask.GetMask("Player", "Bubble"), out var _))
        {
            Attack();
            wait = fsm.StartCoroutine(Wait());
        }
        else
            fsm.ChangeState(ContainerManStateType.Idle);
    }

    void Attack()
    {
        fsm.transform.localScale = new Vector3(fsm.transform.position.x < PlayerFSM.Instance.transform.position.x ? 1 : -1, 1, 1);
        var tire = GameObject.Instantiate(fsm.param.tirePrefab, new Vector3(fsm.transform.position.x, fsm.transform.position.y + 10f), Quaternion.identity).GetComponent<Tire>();
        tire.Init(fsm.transform.localScale.x * Vector2.right, fsm.gameObject);
        tire.Attack();
    }
}
