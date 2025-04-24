using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChickenManStateType
{
    Patrol,
    Attack,
    SprintAttack,
    Covered,
    Dead,
    UnderSwallowed,
    KnockedBack
}

[Serializable]
public class ChickenManParameters
{
    public ChickenManStateType currentState;
    public AnimationCurve sprintAttackCurve;
    public PatrolParameter patrolParameter = new();
    public Action onStart;
}

public class ChickenManFSM : EnemyFSM
{
    public ChickenManParameters parameters;
    public IState currentState;
    public Dictionary<ChickenManStateType, IState> state = new Dictionary<ChickenManStateType, IState>();
    public float attackDirection = new();

    public override void Start()
    {
        base.Start();
        state.Add(ChickenManStateType.Patrol, new ChickenManPatrolState(this));
        state.Add(ChickenManStateType.Attack, new ChickenManAttackState(this));
        state.Add(ChickenManStateType.SprintAttack, new ChickenManSprintAttackState(this));
        state.Add(ChickenManStateType.Covered, new ChickenManCoveredState(this));
        state.Add(ChickenManStateType.UnderSwallowed, new ChickenManUnderSwallowedState(this));
        state.Add(ChickenManStateType.KnockedBack, new ChickenManKnockedBackState(this));


        state.Add(ChickenManStateType.Dead, new ChickenManDeadState(this));
        ChangeState(ChickenManStateType.Patrol);

        GetComponent<SwallowedEnemy>().onLoadActions += () => ChangeState(ChickenManStateType.UnderSwallowed);
        GetComponent<SwallowedEnemy>().onBreakActions += () => ChangeState(ChickenManStateType.Patrol);
        GetComponent<KnockedBackEnemy>().onKnockedBackActions += () => ChangeState(ChickenManStateType.KnockedBack);
    }

    void Update()
    {
        currentState.OnUpdate();
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }



    public void ChangeState(ChickenManStateType stateType)
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
