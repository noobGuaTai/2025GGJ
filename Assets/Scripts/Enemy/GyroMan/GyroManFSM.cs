using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public enum GyroManStateType
{
    Idle,
    Chase,
    Return,
    KnockedBack,
    UnderSwallowed
}

[Serializable]
public class GyroManParameters
{
    public GyroManStateType currentState;
    public float chaseSpeed;
    public float returnSpeed;
    public float detectRange;// 追逐玩家过程中超过该范围则返回原地
    public bool isOnGround => groundCheck.isChecked;
    internal AnythingCheck groundCheck;
}

public class GyroManFSM : EnemyFSM
{
    public GyroManParameters param;
    public IState currentState;
    public Dictionary<GyroManStateType, IState> state = new Dictionary<GyroManStateType, IState>();
    Dictionary<GyroManStateType, Action> enterStateActions = new Dictionary<GyroManStateType, Action>();

    public override void Awake()
    {
        base.Awake();
        initPos = transform.position;
        param.groundCheck = GetComponent<AnythingCheck>();
    }

    public override void Start()
    {
        base.Start();
        state = Enum.GetValues(typeof(GyroManStateType)).Cast<GyroManStateType>().ToDictionary
        (
            stateType => stateType,
            stateType => CreateState(stateType)
        );

        enterStateActions = Enum.GetValues(typeof(GyroManStateType)).Cast<GyroManStateType>().ToDictionary
        (
            stateType => stateType,
            _ => (Action)null
        );

        ChangeState(GyroManStateType.Idle);
    }

    void Update()
    {
        currentState.OnUpdate();
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public void ChangeState(GyroManStateType stateType, Action onEnter = null)
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

    IState CreateState(GyroManStateType stateType)
    {
        switch (stateType)
        {
            case GyroManStateType.Idle: return new GyroManIdleState(this);
            case GyroManStateType.Chase: return new GyroManChaseState(this);
            case GyroManStateType.Return: return new GyroManReturnState(this);
            case GyroManStateType.KnockedBack: return new GyroManKnockedBackState(this);
            case GyroManStateType.UnderSwallowed: return new GyroManUnderSwallowedState(this);
            default: throw new ArgumentException($"Unknown state type: {stateType}");
        }
    }

    public void OnEnter(GyroManStateType stateType)
    {
        enterStateActions[stateType]?.Invoke();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Bubble"))
        {
            BubbleQueue.DestroyBubble(other.gameObject);
        }
    }

}
