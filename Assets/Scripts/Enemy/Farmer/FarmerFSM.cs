using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum FarmerStateType
{
    Idle,
    Patrol,
    Chase,
    Attack,
    KnockedBack,
    UnderSwallowed
}

[Serializable]
public class FarmerParameters
{
    public FarmerStateType currentState;
    public float[] patrolPoint;
    public float patrolSpeed;
    public float patrolMinDistance;
    public float chaseSpeed;
    public float detectRange;// 追逐玩家过程中超过该范围则返回原地
    public float attackDetectRange;// 玩家进入该范围则进入攻击状态
    public float attackRange;
    public Vector2 idleToPatrolTime;
    public Vector2 patrolToIdleTime;
    public bool isOnGround => groundCheck.isChecked;
    internal AnythingCheck groundCheck;
    public Collider2D attackCollider;
}

public class FarmerFSM : EnemyFSM
{
    public FarmerParameters param;
    public IState currentState;
    public Dictionary<FarmerStateType, IState> state = new Dictionary<FarmerStateType, IState>();
    public Dictionary<FarmerStateType, Action> enterStateActions = new Dictionary<FarmerStateType, Action>();

    public override void Awake()
    {
        base.Awake();
        param.groundCheck = GetComponent<AnythingCheck>();
    }

    public override void Start()
    {
        base.Start();
        state = Enum.GetValues(typeof(FarmerStateType)).Cast<FarmerStateType>().ToDictionary(
            stateType => stateType,
            stateType => CreateState(stateType)
        );

        enterStateActions = Enum.GetValues(typeof(FarmerStateType)).Cast<FarmerStateType>().ToDictionary(
            stateType => stateType,
            _ => (Action)null
        );
        ChangeState(FarmerStateType.Idle);
    }

    void Update()
    {
        currentState.OnUpdate();
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public void ChangeState(FarmerStateType stateType, Action onEnter = null)
    {
        if (onEnter != null)
            if (enterStateActions.ContainsKey(stateType))
                enterStateActions[stateType] += onEnter;
            else
                enterStateActions.Add(stateType, onEnter);

        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = state[stateType];
        currentState.OnEnter();
        param.currentState = stateType;
    }

    IState CreateState(FarmerStateType stateType)
    {
        switch (stateType)
        {
            case FarmerStateType.Idle: return new FarmerIdleState(this);
            case FarmerStateType.Patrol: return new FarmerPatrolState(this);
            case FarmerStateType.Chase: return new FarmerChaseState(this);
            case FarmerStateType.Attack: return new FarmerAttackState(this);
            case FarmerStateType.KnockedBack: return new FarmerKnockedBackState(this);
            case FarmerStateType.UnderSwallowed: return new FarmerUnderSwallowedState(this);
            default: throw new ArgumentException($"Unknown state type: {stateType}");
        }
    }

    public void OnEnter(FarmerStateType type)
    {
        enterStateActions[type]?.Invoke();
    }
}