using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerAttackState : IState
{
    private FarmerFSM fsm;
    Coroutine wait;
    public FarmerAttackState(FarmerFSM fsm)
    {
        this.fsm = fsm;
    }

    public void OnEnter()
    {
        fsm.rb.linearVelocity = Vector2.zero;
        wait = fsm.StartCoroutine(Wait());
        var s = GameObject.Instantiate(fsm.param.sicklePrefab, new Vector3(fsm.transform.position.x, fsm.transform.position.y + 10f), Quaternion.identity).GetComponent<Sickle>();
        s.Init(fsm.transform.localScale.x * Vector2.right, fsm.gameObject);
        s.Attack();
        fsm.param.currentSickle = s.gameObject;
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
        fsm.ChangeState(FarmerStateType.Idle);
    }
}
