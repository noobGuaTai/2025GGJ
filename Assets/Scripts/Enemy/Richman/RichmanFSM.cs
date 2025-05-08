using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum RichmanStateType
{
    Idle,
}

[Serializable]
public class RichmanParameters
{
    public RichmanStateType currentState;
    public int needCoinNum = 1;
    public int currentHasCoin = 0;
}

public class RichmanFSM : EnemyFSM
{
    public RichmanParameters param;
    public IState currentState;
    public Dictionary<RichmanStateType, IState> state = new Dictionary<RichmanStateType, IState>();
    Dictionary<RichmanStateType, Action> enterStateActions = new Dictionary<RichmanStateType, Action>();

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        state = Enum.GetValues(typeof(RichmanStateType)).Cast<RichmanStateType>().ToDictionary
        (
            stateType => stateType,
            stateType => CreateState(stateType)
        );

        enterStateActions = Enum.GetValues(typeof(RichmanStateType)).Cast<RichmanStateType>().ToDictionary
        (
            stateType => stateType,
            _ => (Action)null
        );
        
        ChangeState(RichmanStateType.Idle);
    }

    void Update()
    {
        currentState.OnUpdate();
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public void ChangeState(RichmanStateType stateType, Action onEnter = null)
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

    IState CreateState(RichmanStateType stateType)
    {
        switch (stateType)
        {
            case RichmanStateType.Idle: return new RichmanIdleState(this);
            default: throw new ArgumentException($"Unknown state type: {stateType}");
        }
    }

    public void OnEnter(RichmanStateType stateType) => enterStateActions[stateType]?.Invoke();
    public Door door;
    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.gameObject.TryGetComponent<WeaponCoin>(out var wc))
        {
            Destroy(other.gameObject);
            param.currentHasCoin++;
            if (param.currentHasCoin >= param.needCoinNum)
                door.Open();
        }      
    }
    public override void Die()
    {
        base.Die();
        GameManager.Instance.RichmanKilled = true;

        door.Open();
    }

}
