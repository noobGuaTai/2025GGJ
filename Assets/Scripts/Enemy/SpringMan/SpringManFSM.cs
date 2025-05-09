using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SpringManStateType
{
    Idle,
    Chase,
    Patrol,
    KnockedBack,
    UnderSwallowed
}

[Serializable]
public class SpringManParameters
{
    public SpringManStateType currentState;
    public float[] patrolPoint;
    public float patrolSpeed;
    public float patrolMinDistance;
    public float chaseSpeed;
    public float springForce;
    internal float springTimer;
    public float springCoolDown;
    public float sprintHeavyThingForce;
    public float detectRange;// 追逐玩家过程中超过该范围则返回原地
    public float attackDetectRange;// 追逐玩家过程中超过该范围则返回原地
    public Vector2 idleToPatrolTime;
    public Vector2 patrolToIdleTime;
    public bool isOnGround => groundCheck.isChecked;
    internal AnythingCheck groundCheck;
}

public class SpringManFSM : EnemyFSM
{
    public SpringManParameters param;
    public IState currentState;
    public Dictionary<SpringManStateType, IState> state = new Dictionary<SpringManStateType, IState>();
    Dictionary<SpringManStateType, Action> enterStateActions = new Dictionary<SpringManStateType, Action>();

    public override void Awake()
    {
        base.Awake();
        param.groundCheck = GetComponent<AnythingCheck>();
    }

    public override void Start()
    {
        base.Start();
        state = Enum.GetValues(typeof(SpringManStateType)).Cast<SpringManStateType>().ToDictionary
        (
            stateType => stateType,
            stateType => CreateState(stateType)
        );

        enterStateActions = Enum.GetValues(typeof(SpringManStateType)).Cast<SpringManStateType>().ToDictionary
        (
            stateType => stateType,
            _ => (Action)null
        );

        ChangeState(SpringManStateType.Idle);
        GetComponent<SwallowedEnemy>().onLoadActions += () => ChangeState(SpringManStateType.UnderSwallowed);
        GetComponent<SwallowedEnemy>().onBreakActions += () => ChangeState(SpringManStateType.Patrol);
        GetComponent<KnockedBackEnemy>().onKnockedBackActions += () => ChangeState(SpringManStateType.KnockedBack);
        GetComponentInChildren<EnemyAttackAnything>().onAttacked += (other) =>
        {
            if (other.TryGetComponent<SmallBubble>(out var s))
            {
                s.isBeingDestroyed = true;
                BubbleQueue.DestroyBubble(other.gameObject);
            }
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                PlayerFSM.Instance.Die();
            }
        };
    }

    void Update()
    {
        currentState.OnUpdate();
        Spring();
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public void ChangeState(SpringManStateType stateType, Action onEnter = null)
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

    IState CreateState(SpringManStateType stateType)
    {
        switch (stateType)
        {
            case SpringManStateType.Idle: return new SpringManIdleState(this);
            case SpringManStateType.Chase: return new SpringManChaseState(this);
            case SpringManStateType.Patrol: return new SpringManPatrolState(this);
            case SpringManStateType.KnockedBack: return new SpringManKnockedBackState(this);
            case SpringManStateType.UnderSwallowed: return new SpringManUnderSwallowedState(this);
            default: throw new ArgumentException($"Unknown state type: {stateType}");
        }
    }

    public void OnEnter(SpringManStateType stateType) => enterStateActions[stateType]?.Invoke();

    void Spring()
    {
        if (param.isOnGround && (currentState == state[SpringManStateType.Patrol] || currentState == state[SpringManStateType.Chase]) && param.springTimer > param.springCoolDown)
        {
            param.springTimer = 0;
            animator.Play("jump", 0, 0);
            attackAudio.Play();
        }
        param.springTimer += Time.deltaTime;
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        if (other.gameObject.TryGetComponent<EnemyFSM>(out var e))
        {
            if (e.somatoType == EnemySomatoType.Heavy)
            {
                float angle = Vector2.Angle(Vector2.up, (other.transform.position - transform.position).normalized);
                if (angle < 60f)
                {
                    e.rb.AddForce(new Vector2(0, param.sprintHeavyThingForce), ForceMode2D.Impulse);
                }
            }
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            float angle = Vector2.Angle(Vector2.up, (other.transform.position - transform.position).normalized);
            if (angle < 60f)
            {
                PlayerFSM.Instance.ChangeState(PlayerStateType.Jump);
            }
        }

    }
    public void SpringAddForce() => rb.AddForce(new Vector2(0, param.springForce), ForceMode2D.Impulse);
}
