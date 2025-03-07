using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MushroomManStateType
{
    Patrol,
    Attack
}

[Serializable]
public class MushroomManParameters
{
    public MushroomManStateType currentState;
    public Vector2[] patrolPoint;
    public float patrolSpeed;
    public float chaseSpeed;
    public float detectRange;// 追逐玩家过程中超过该范围则返回原地
    public float attackRange;// 玩家进入该范围则进入攻击状态
}

public class MushroomManFSM : EnemyFSM
{
    public MushroomManParameters parameters;
    public IState currentState;
    public Dictionary<MushroomManStateType, IState> state = new Dictionary<MushroomManStateType, IState>();

    public override void Start()
    {
        base.Start();
        state.Add(MushroomManStateType.Patrol, new MushroomManPatrolState(this));
        state.Add(MushroomManStateType.Attack, new MushroomManAttackState(this));
        ChangeState(MushroomManStateType.Patrol);
    }

    void Update()
    {
        currentState.OnUpdate();
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public void ChangeState(MushroomManStateType stateType)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = state[stateType];
        currentState.OnEnter();
        parameters.currentState = stateType;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

        }
    }
}
