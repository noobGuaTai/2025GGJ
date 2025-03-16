using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public enum MushroomManStateType
{
    Patrol,
    Attack,
    UnderSwallowed,
    KnockedBack
}

[Serializable]
public class MushroomManParameters
{
    public MushroomManStateType currentState;
    public float[] patrolPoint;
    public float patrolSpeed;
    public float chaseSpeed;
    public float detectRange;// 追逐玩家过程中超过该范围则返回原地
    public float attackRange;// 玩家进入该范围则进入攻击状态
    public LayerMask deadlyLayers;
    public bool isOnGround => groundCheck.isChecked;
    internal AnythingCheck groundCheck;
}

public class MushroomManFSM : EnemyFSM
{
    public MushroomManParameters parameters;
    public IState currentState;
    public Dictionary<MushroomManStateType, IState> state = new Dictionary<MushroomManStateType, IState>();

    public override void Awake()
    {
        base.Awake();
        parameters.groundCheck = GetComponent<AnythingCheck>();
    }

    public override void Start()
    {
        base.Start();
        state.Add(MushroomManStateType.Patrol, new MushroomManPatrolState(this));
        state.Add(MushroomManStateType.Attack, new MushroomManAttackState(this));
        state.Add(MushroomManStateType.UnderSwallowed, new MushroomManUnderSwallowedState(this));
        state.Add(MushroomManStateType.KnockedBack, new MushroomManKnockedBackState(this));
        ChangeState(MushroomManStateType.Patrol);

        GetComponent<SwallowedEnemy>().onLoadActions += () => ChangeState(MushroomManStateType.UnderSwallowed);
        GetComponent<SwallowedEnemy>().onBreakActions += () => ChangeState(MushroomManStateType.Patrol);
        GetComponent<KnockedBackEnemy>().onKnockedBackActions += () => ChangeState(MushroomManStateType.KnockedBack);


    }

    void Update()
    {
        currentState.OnUpdate();
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public void ChangeState(MushroomManStateType stateType)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = state[stateType];
        currentState.OnEnter();
        parameters.currentState = stateType;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (((1 << other.gameObject.layer) & parameters.deadlyLayers) != 0)
        {
            Die();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, parameters.detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, parameters.attackRange);
    }

    public override void Die()
    {

        base.Die();
    }
}
