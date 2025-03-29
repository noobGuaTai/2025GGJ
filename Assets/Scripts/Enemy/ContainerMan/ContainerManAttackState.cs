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

        fsm.rb.linearVelocity = Vector2.zero;
        wait = fsm.StartCoroutine(Wait());
        Attack();


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

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
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
