using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public enum BovineManStateType
{
    Patrol,
    SprintAttack,   // 冲刺攻击
    Braking,    // 刹车
    ChargedEnergy,   // 蓄力
    UnderSwallowed,   // 被吞下
}

[Serializable]
public class BovineManParameters
{
    public BovineManStateType currentState;
    // [HideInInspector] public float currentSpeed;
    public float currentSpeed;

    [Header("Patrol")]
    public Vector2[] patrolPoint;
    public float patrolSpeed;

    [Header("SprintAttack")]
    public float sprintBaseSpeed;     // 冲刺基础速度
    public float speedIncrementOnChargingPerSec;  // 蓄力每秒基础速度增量
    [HideInInspector] public int sprintDirection;   // 冲刺方向
    public float maxVelocity; // 最大速度
    public float canCancelSprintDuration; // 检测不到玩家多少秒后停止冲刺

    [Header("ChargingEnergy")]
    public float chargingDurationUpper;  // 冲刺蓄力时间上限
    public float chargingDurationLower;  // 冲刺蓄力时间下限
    public float canCancelChargingDuration; // 检测不到玩家多少秒后停止蓄力
    [HideInInspector] public float chargingDuration;  // 冲刺蓄力时间
    public float acceleratedVelocity; // 加速度

    [Header("Braking")]
    public float retardedVelocity;  // 减速度

    [Header("Detection")]
    public float attackDetectRange;   // 追逐玩家过程中超过该范围则返回原地
    public float returnDetectRange;   // 玩家进入该范围则进入蓄力状态
    public LayerMask deadlyLayers;
 }

 public class BovineManFSM : EnemyFSM
 {
    public BovineManParameters parameters;
    public IState currentState;
    public Dictionary<BovineManStateType, IState> state = new Dictionary<BovineManStateType, IState>();

    public override void Start()
    {
        base.Start();
        state.Add(BovineManStateType.Patrol, new BovineManPatrolState(this));
        state.Add(BovineManStateType.ChargedEnergy, new BovineManChargedEnergyState(this));
        state.Add(BovineManStateType.SprintAttack, new BovineManSprintAttackState(this));
        state.Add(BovineManStateType.Braking, new BovineManBrakingState(this));
        state.Add(BovineManStateType.UnderSwallowed, new BovineManUnderSwallowedState(this));

        ChangeState(BovineManStateType.Patrol);

        GetComponent<SwallowedEnemy>().onLoadActions += () => ChangeState(BovineManStateType.UnderSwallowed);
        GetComponent<SwallowedEnemy>().onBreakActions += () => ChangeState(BovineManStateType.Patrol);
        // #TODO: 撞击伤害
        // #TODO: 被高速硬币撞击
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
        parameters.currentState = stateType;
        Debug.Log("BovineMan ChangeState: " + stateType);
    }

    public bool IsPlayerInFront(float range) =>
        Physics2D.Raycast(transform.position, new Vector2(0, parameters.sprintDirection), range, 1 << LayerMask.NameToLayer("Player")).collider != null;

    public void Braking() => ChasePlayer();

    public void ChasePlayer() => ChasePlayer(parameters.currentSpeed);

    public void ChasePlayer(float speed) => rb.linearVelocityX = parameters.sprintDirection * speed;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, parameters.returnDetectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, parameters.attackDetectRange);
    }

   public bool DetectPlayer(float attackDetectRange) =>
        IsDetectObjectByLayer(parameters.attackDetectRange, LayerMask.GetMask("Player"), out var _);
}