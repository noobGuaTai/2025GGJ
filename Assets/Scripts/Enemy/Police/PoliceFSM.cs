using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PoliceStateType
{
    Idle,
    Patrol,
    Chase,
    Attack,
    KnockedBack,
    UnderSwallowed,
    Disarm
}

[Serializable]
public class PoliceParameters
{
    public PoliceStateType currentState;
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
    public Collider2D[] attackCollider;
}

public class PoliceFSM : EnemyFSM
{
    public PoliceParameters param;
    public IState currentState;
    public Dictionary<PoliceStateType, IState> state = new Dictionary<PoliceStateType, IState>();
    Dictionary<PoliceStateType, Action> enterStateActions = new Dictionary<PoliceStateType, Action>();
    static HashSet<GameObject> AllPolice = new();
    static void RiseAllPolice()
    {
        AllPolice.ToList().ForEach(x => x.SetActive(true));
    }

    public override void Awake()
    {
        base.Awake();
        param.groundCheck = GetComponent<AnythingCheck>();
    }

    public override void Start()
    {
        base.Start();
        state = Enum.GetValues(typeof(PoliceStateType)).Cast<PoliceStateType>().ToDictionary
        (
            stateType => stateType,
            stateType => CreateState(stateType)
        );

        enterStateActions = Enum.GetValues(typeof(PoliceStateType)).Cast<PoliceStateType>().ToDictionary
        (
            stateType => stateType,
            _ => (Action)null
        );

        ChangeState(PoliceStateType.Idle);
        GetComponent<SwallowedEnemy>().onLoadActions += () => ChangeState(PoliceStateType.UnderSwallowed);
        GetComponent<SwallowedEnemy>().onBreakActions += () => ChangeState(PoliceStateType.Patrol);
        GetComponent<KnockedBackEnemy>().onKnockedBackActions += () => ChangeState(PoliceStateType.KnockedBack);
        var a = GetComponentInChildren<EnemyAttackAnything>();
        a.onAttacked += Attack;
    }

    void Update()
    {
        currentState.OnUpdate();
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public void ChangeState(PoliceStateType stateType, Action onEnter = null)
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

    IState CreateState(PoliceStateType stateType)
    {
        switch (stateType)
        {
            case PoliceStateType.Idle: return new PoliceIdleState(this);
            case PoliceStateType.Patrol: return new PolicePatrolState(this);
            case PoliceStateType.Chase: return new PoliceChaseState(this);
            case PoliceStateType.Attack: return new PoliceAttackState(this);
            case PoliceStateType.KnockedBack: return new PoliceKnockedBackState(this);
            case PoliceStateType.UnderSwallowed: return new PoliceUnderSwallowedState(this);
            case PoliceStateType.Disarm: return new PoliceDisarmState(this);
            default: throw new ArgumentException($"Unknown state type: {stateType}");
        }
    }

    public void OnEnter(PoliceStateType stateType)
    {
        enterStateActions[stateType]?.Invoke();
    }

    public void EnableAttackCollider() => Array.ForEach(param.attackCollider, x => x.enabled = true);
    public void DisableAttackCollider() => Array.ForEach(param.attackCollider, x => x.enabled = false);


    public void Attack(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<BaseBubble>(out var _))
        {
            BubbleQueue.DestroyBubble(other.gameObject);
        }

        if (other.CompareTag("Player"))
        {
            PlayerFSM.Instance.Die();
        }
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Coin"))
        {
            Destroy(other.gameObject);
        }
    }
    public void OnDestroy()
    {
        AllPolice.Remove(gameObject);
    }
}
