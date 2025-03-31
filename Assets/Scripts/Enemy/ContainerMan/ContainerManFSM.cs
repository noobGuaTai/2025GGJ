using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ContainerManStateType
{
    Idle,
    Patrol,
    Chase,
    Attack,
    KnockedBack,
    UnderSwallowed
}

[Serializable]
public class ContainerManParameters
{
    public ContainerManStateType currentState;
    public float[] patrolPoint;
    public float patrolSpeed;
    public float chaseSpeed;
    public float detectRange;// 追逐玩家过程中超过该范围则返回原地
    public float attackDetectRange;// 玩家进入该范围则进入攻击状态
    public float attackRange;
    public Vector2 idleToPatrolTime;
    public Vector2 patrolToIdleTime;
    public bool isOnGround => groundCheck.isChecked;
    internal AnythingCheck groundCheck;
    public GameObject tirePrefab;
}

public class ContainerManFSM : EnemyFSM
{
    public ContainerManParameters param;
    public IState currentState;
    public Dictionary<ContainerManStateType, IState> state = new Dictionary<ContainerManStateType, IState>();
    Dictionary<ContainerManStateType, Action> enterStateActions = new Dictionary<ContainerManStateType, Action>();

    public override void Awake()
    {
        base.Awake();
        param.groundCheck = GetComponent<AnythingCheck>();
    }

    public override void Start()
    {
        base.Start();
        state = Enum.GetValues(typeof(ContainerManStateType)).Cast<ContainerManStateType>().ToDictionary
        (
            stateType => stateType,
            stateType => CreateState(stateType)
        );

        enterStateActions = Enum.GetValues(typeof(ContainerManStateType)).Cast<ContainerManStateType>().ToDictionary
        (
            stateType => stateType,
            _ => (Action)null
        );

        ChangeState(ContainerManStateType.Idle);
        GetComponent<SwallowedEnemy>().onLoadActions += () => ChangeState(ContainerManStateType.UnderSwallowed);
        GetComponent<SwallowedEnemy>().onBreakActions += () => ChangeState(ContainerManStateType.Idle);
        GetComponent<KnockedBackEnemy>().onKnockedBackActions += () => ChangeState(ContainerManStateType.KnockedBack);
    }

    void Update()
    {
        currentState.OnUpdate();
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public void ChangeState(ContainerManStateType stateType, Action onEnter = null)
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

    IState CreateState(ContainerManStateType stateType)
    {
        switch (stateType)
        {
            case ContainerManStateType.Idle: return new ContainerManIdleState(this);
            case ContainerManStateType.Patrol: return new ContainerManPatrolState(this);
            case ContainerManStateType.Chase: return new ContainerManChaseState(this);
            case ContainerManStateType.Attack: return new ContainerManAttackState(this);
            case ContainerManStateType.KnockedBack: return new ContainerManKnockedBackState(this);
            case ContainerManStateType.UnderSwallowed: return new ContainerManUnderSwallowedState(this);
            default: throw new ArgumentException($"Unknown state type: {stateType}");
        }
    }

    public void OnEnter(ContainerManStateType stateType) => enterStateActions[stateType]?.Invoke();
}
