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
        fsm.animator.Play("attack", 0, 0);
        yield return new WaitForSeconds(0.2f);
        // SpawnEnemy();
        yield return new WaitForSeconds(fsm.param.spawnInterval);
    }

    void SpawnEnemy()
    {
        var spawnPos = fsm.param.spawnPoints[Random.Range(0, fsm.param.spawnPoints.Length)].position;
        GameObject.Instantiate(fsm.param.smoke, spawnPos, Quaternion.identity);
        var g = GameObject.Instantiate(fsm.param.enemys[Random.Range(0, fsm.param.enemys.Length)], spawnPos, Quaternion.identity);
        g.transform.parent = fsm.transform;
    }
}
