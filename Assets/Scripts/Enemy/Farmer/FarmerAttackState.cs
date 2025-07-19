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

        if (fsm.param.currentSickle != null)
            return;
        fsm.transform.localScale = new Vector3(fsm.transform.position.x < PlayerFSM.Instance.transform.position.x ? 1 : -1, 1, 1);
        fsm.animator.Play("attack", 0, 0);

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
