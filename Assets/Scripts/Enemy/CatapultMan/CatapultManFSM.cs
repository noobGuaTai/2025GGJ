using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CatapultManStateType
{
    Idle,
    Patrol,
    Chase,
    Attack,
    KnockedBack,
    UnderSwallowed
}

[Serializable]
public class CatapultManParameters
{
    public CatapultManStateType currentState;
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
    public GameObject rockPrefab;
}

public class CatapultManFSM : EnemyFSM
{
    public CatapultManParameters param;
    public IState currentState;
    public Dictionary<CatapultManStateType, IState> state = new Dictionary<CatapultManStateType, IState>();
    Dictionary<CatapultManStateType, Action> enterStateActions = new Dictionary<CatapultManStateType, Action>();

    public override void Awake()
    {
        base.Awake();
        param.groundCheck = GetComponent<AnythingCheck>();
    }

    public override void Start()
    {
        base.Start();
        state = Enum.GetValues(typeof(CatapultManStateType)).Cast<CatapultManStateType>().ToDictionary
        (
            stateType => stateType,
            stateType => CreateState(stateType)
        );

        enterStateActions = Enum.GetValues(typeof(CatapultManStateType)).Cast<CatapultManStateType>().ToDictionary
        (
            stateType => stateType,
            _ => (Action)null
        );

        ChangeState(CatapultManStateType.Idle);
        GetComponent<SwallowedEnemy>().onLoadActions += () => ChangeState(CatapultManStateType.UnderSwallowed);
        GetComponent<SwallowedEnemy>().onBreakActions += () => ChangeState(CatapultManStateType.Idle);
        GetComponent<KnockedBackEnemy>().onKnockedBackActions += () => ChangeState(CatapultManStateType.KnockedBack);
    }

    void Update()
    {
        currentState.OnUpdate();
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public void ChangeState(CatapultManStateType stateType, Action onEnter = null)
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

    IState CreateState(CatapultManStateType stateType)
    {
        switch (stateType)
        {
            case CatapultManStateType.Idle: return new CatapultManIdleState(this);
            case CatapultManStateType.Patrol: return new CatapultManPatrolState(this);
            case CatapultManStateType.Chase: return new CatapultManChaseState(this);
            case CatapultManStateType.Attack: return new CatapultManAttackState(this);
            case CatapultManStateType.KnockedBack: return new CatapultManKnockedBackState(this);
            case CatapultManStateType.UnderSwallowed: return new CatapultManUnderSwallowedState(this);
            default: throw new ArgumentException($"Unknown state type: {stateType}");
        }
    }

    public void OnEnter(CatapultManStateType stateType) => enterStateActions[stateType]?.Invoke();
}
