using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : IState
{
    BossFSM fsm;

    public BossIdleState(BossFSM fsm) => this.fsm = fsm;

    public void OnEnter()
    {
        fsm.OnEnter(BossStateType.Idle);
        fsm.StartCoroutine(SpawnEnemys());
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

    IEnumerator SpawnEnemys()
    {
        while (true)
        {
            fsm.animator.Play("attack", 0, 0);
            yield return new WaitForSeconds(fsm.param.spawnInterval);
        }
    }

}
