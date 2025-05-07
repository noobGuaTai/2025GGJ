using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public enum BovineManStateType
{
    Patrol,
    Chase,  // 攻击 追击
    ChargedEnergy,   // 蓄力
    UnderSwallowed,   // 被吞下
    Return,   // 返回
}

[Serializable]
public class BovineManParameters
{
    public BovineManStateType currentState;
    // [HideInInspector] public float currentSpeed;
    public float chaseSpeed;

    [Header("Patrol")]
    // public Vector2[] patrolPoint;
    public float[] patrolPoint;
    public float patrolSpeed;

    [Header("SprintAttack")]
    public float sprintBaseSpeed;     // 冲刺基础速度

    [Header("ChargingEnergy")]
    public float chargingDurationUpper;  // 冲刺蓄力时间上限
    public float chargingDurationLower;  // 冲刺蓄力时间下限
    public float speedIncrementOnChargingPerSec;  // 蓄力每秒基础速度增量
    public float canCancelChargingDuration; // 检测不到玩家多少秒后停止蓄力
    [HideInInspector] public float chargingDuration;  // 冲刺蓄力时间

    [Header("Braking")]
    public float decelerateSpeed;  // 减速度

    [Header("Detection")]
    public float detectRange;   // 玩家进入该范围则进入蓄力状态
    public float returnDetectRange;   // 追逐玩家过程中超过该范围则返回原地
    public float returnSpeed;  // 返回原地的速度
    public bool isOnGround => groundCheck.isChecked;
    internal AnythingCheck groundCheck;

}

public class BovineManFSM : EnemyFSM
{
    public BovineManParameters param;
    public IState currentState;
    public Dictionary<BovineManStateType, IState> state = new Dictionary<BovineManStateType, IState>();

    public override void Awake()
    {
        base.Awake();
        initPos = transform.position;
        param.groundCheck = GetComponentInChildren<AnythingCheck>();
    }

    public override void Start()
    {
        base.Start();
        state.Add(BovineManStateType.Patrol, new BovineManPatrolState(this));
        state.Add(BovineManStateType.ChargedEnergy, new BovineManChargedEnergyState(this));
        state.Add(BovineManStateType.Chase, new BovineManChaseState(this));
        state.Add(BovineManStateType.UnderSwallowed, new BovineManUnderSwallowedState(this));
        state.Add(BovineManStateType.Return, new BovineManReturnState(this));

        ChangeState(BovineManStateType.Patrol);

        GetComponent<SwallowedEnemy>().onLoadActions += () => ChangeState(BovineManStateType.UnderSwallowed);
        GetComponent<SwallowedEnemy>().onBreakActions += () => ChangeState(BovineManStateType.Patrol);

        // GetComponentInChildren<EnemyAttackAnything>().onAttacked += (other) => { if (other.TryGetComponent<SmallBubble>(out var s)) s.isBeingDestroyed = true; BubbleQueue.DestroyBubble(other.gameObject); };
    }

    void Update()
    {
        currentState.OnUpdate();
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public void ChangeState(BovineManStateType stateType)
    {
        currentState?.OnExit();
        currentState = state[stateType];
        currentState.OnEnter();
        param.currentState = stateType;
        Debug.Log("BovineMan ChangeState: " + stateType);
    }

    // # TODO: 调试信息可能需要修改
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, param.returnDetectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, param.detectRange);
    }

}