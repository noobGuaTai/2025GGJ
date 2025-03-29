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
        yield return new WaitForSeconds(1f);
        fsm.ChangeState(CatapultManStateType.Idle);
    }

    void Attack()
    {
        fsm.transform.localScale = new Vector3(fsm.transform.position.x < PlayerFSM.Instance.transform.position.x ? 1 : -1, 1, 1);
        var tire = GameObject.Instantiate(fsm.param.rockPrefab, new Vector3(fsm.transform.position.x, fsm.transform.position.y + 10f), Quaternion.identity).GetComponent<Rock>();
        tire.Init(fsm.transform.localScale.x * Vector2.right, fsm.gameObject, PlayerFSM.Instance.transform.position);
        tire.Attack();
    }
}
