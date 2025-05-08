using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BossStateType
{
    Idle,
    Attack,
}

[Serializable]
public class BossParameters
{
    public BossStateType currentState;
    public GameObject[] enemys;
    public float spawnInterval;
    public Transform[] spawnPoints;
    public GameObject spawnPointsParent;
    public GameObject smoke;
}

public class BossFSM : EnemyFSM
{
    public BossParameters param;
    public IState currentState;
    public Dictionary<BossStateType, IState> state = new Dictionary<BossStateType, IState>();
    Dictionary<BossStateType, Action> enterStateActions = new Dictionary<BossStateType, Action>();

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        state = Enum.GetValues(typeof(BossStateType)).Cast<BossStateType>().ToDictionary
        (
            stateType => stateType,
            stateType => CreateState(stateType)
        );

        enterStateActions = Enum.GetValues(typeof(BossStateType)).Cast<BossStateType>().ToDictionary
        (
            stateType => stateType,
            _ => (Action)null
        );

        ChangeState(BossStateType.Idle);

        param.spawnPoints = param.spawnPointsParent.GetComponentsInChildren<Transform>().Where(x => x != param.spawnPointsParent.transform).ToArray();
    }

    void Update()
    {
        currentState.OnUpdate();
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public void ChangeState(BossStateType stateType, Action onEnter = null)
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

    IState CreateState(BossStateType stateType)
    {
        switch (stateType)
        {
            case BossStateType.Idle: return new BossIdleState(this);
            case BossStateType.Attack: return new BossAttackState(this);
            default: throw new ArgumentException($"Unknown state type: {stateType}");
        }
    }

    public void OnEnter(BossStateType stateType) => enterStateActions[stateType]?.Invoke();
    public override void Die()
    {
        base.Die();
        GameManager.Instance.GameOver();
    }
}
