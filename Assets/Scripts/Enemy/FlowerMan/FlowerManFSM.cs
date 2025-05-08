using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlowerManStateType
{
    Patrol,
    Attack,
    Return,
    KnockedBack,
    UnderSwallowed
}

[Serializable]
public class FlowerManParameters
{
    public FlowerManStateType currentState;
    public PatrolParameter patrolParameter = new();
    public float returnSpeed;
    public GameObject flowerProjectilePrefab;
    public float flowerSpeed = 10f;
    public float shootCD = 0.5f;
    public bool isOnGround => groundCheck.isChecked;
    internal AnythingCheck groundCheck;
    public float detectRange;// 追逐玩家过程中超过该范围则返回原地
    public float attackRange;// 玩家进入该范围则进入攻击状态
    public float shootTimer;
    public GameObject flowerProjectilePrefabSpawnPoint;
    public GameObject[] flowers;

}

public class FlowerManFSM : EnemyFSM
{
    public FlowerManParameters parameters;
    public IState currentState;
    public Dictionary<FlowerManStateType, IState> state = new Dictionary<FlowerManStateType, IState>();

    public override void Start()
    {
        base.Start();
        state.Add(FlowerManStateType.Patrol, new FlowerManPatrolState(this));
        state.Add(FlowerManStateType.Attack, new FlowerManAttackState(this));
        state.Add(FlowerManStateType.Return, new FlowerManReturnState(this));
        state.Add(FlowerManStateType.KnockedBack, new FlowerManKnockedBackState(this));
        state.Add(FlowerManStateType.UnderSwallowed, new FlowerManUnderSwallowedState(this));
        ChangeState(FlowerManStateType.Patrol);
        GetComponent<SwallowedEnemy>().onLoadActions += () => ChangeState(FlowerManStateType.UnderSwallowed);
        GetComponent<SwallowedEnemy>().onBreakActions += () => ChangeState(FlowerManStateType.Patrol);
        GetComponent<KnockedBackEnemy>().onKnockedBackActions += () => ChangeState(FlowerManStateType.KnockedBack);

        parameters.groundCheck = GetComponentInChildren<AnythingCheck>();
    }

    void Update()
    {
        currentState.OnUpdate();
        parameters.shootTimer += Time.deltaTime;
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public void ChangeState(FlowerManStateType stateType)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = state[stateType];
        currentState.OnEnter();
        parameters.currentState = stateType;
    }

    public void Attack(GameObject aim)
    {
        var g = Instantiate(parameters.flowers[UnityEngine.Random.Range(0, parameters.flowers.Length)], transform.position, Quaternion.identity);
        g.GetComponent<Rigidbody2D>().linearVelocity = ((aim.transform.position - transform.position).normalized * parameters.flowerSpeed);
        g.transform.parent = transform;
    }
}
