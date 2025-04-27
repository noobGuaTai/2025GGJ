using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlowerManStateType
{
    Patrol,
    Attack,
    Return
}

[Serializable]
public class FlowerManParameters
{
    public FlowerManStateType currentState;
    public PatrolParameter patrolParameter = new();
    public float returnSpeed;
    public GameObject flowerProjectile;
    public float flowerSpeed = 10f;
    public float shootCD = 0.5f;

}

public class FlowerManFSM : EnemyFSM
{
    public FlowerManParameters parameters;
    public IState currentState;
    public Dictionary<FlowerManStateType, IState> state = new Dictionary<FlowerManStateType, IState>();

    public override void Start()
    {
        base.Start();
        state.Add(FlowerManStateType.Patrol, new FlowerManPatrolState(this));
        state.Add(FlowerManStateType.Attack, new FlowerManAttackState(this));
        state.Add(FlowerManStateType.Return, new FlowerManReturnState(this));
        ChangeState(FlowerManStateType.Patrol);
    }

    void Update()
    {
        currentState.OnUpdate();
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public void ChangeState(FlowerManStateType stateType)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = state[stateType];
        currentState.OnEnter();
        parameters.currentState = stateType;
    }
}
