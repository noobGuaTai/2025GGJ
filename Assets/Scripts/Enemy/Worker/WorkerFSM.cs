using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorkerStateType
{
    Idle,
    Patrol,
    Attack,
    UnderSwallowed,
    KnockedBack,
    Chase
}

[Serializable]
public class WorkerParameters
{
    public WorkerStateType currentState;
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

public class WorkerFSM : EnemyFSM
{
    public WorkerParameters param;
    public IState currentState;
    public Dictionary<WorkerStateType, IState> state = new Dictionary<WorkerStateType, IState>();

    public override void Awake()
    {
        base.Awake();
        param.groundCheck = GetComponent<AnythingCheck>();
    }

    public override void Start()
    {
        base.Start();
        state.Add(WorkerStateType.Idle, new WorkerIdleState(this));
        state.Add(WorkerStateType.Patrol, new WorkerPatrolState(this));
        state.Add(WorkerStateType.Attack, new WorkerAttackState(this));
        state.Add(WorkerStateType.UnderSwallowed, new WorkerUnderSwallowedState(this));
        state.Add(WorkerStateType.KnockedBack, new WorkerKnockedBackState(this));
        state.Add(WorkerStateType.Chase, new WorkerChaseState(this));


        ChangeState(WorkerStateType.Idle);

        GetComponent<SwallowedEnemy>().onLoadActions += () => ChangeState(WorkerStateType.UnderSwallowed);
        GetComponent<SwallowedEnemy>().onBreakActions += () => ChangeState(WorkerStateType.Patrol);
        GetComponent<KnockedBackEnemy>().onKnockedBackActions += () => ChangeState(WorkerStateType.KnockedBack);
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

    public void ChangeState(WorkerStateType stateType)
    {
        // if (currentState == state[WorkerStateType.UnderSwallowed])
        //     return;
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = state[stateType];
        currentState.OnEnter();
        param.currentState = stateType;
    }

    public void EnableAttackCollider() => param.attackCollider.enabled = true;
    public void DisableAttackCollider() => param.attackCollider.enabled = false;


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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, param.detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, param.attackRange);
    }

}
