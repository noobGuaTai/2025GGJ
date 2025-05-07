using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BorerManStateType
{
    Idle,
    Attack,
    KnockedBack,
    UnderSwallowed
}

[Serializable]
public class BorerManParameters
{
    public BorerManStateType currentState;
    public float detectRange;// 追逐玩家过程中超过该范围则返回原地
    public float attackDetectRange;// 玩家进入该范围则进入攻击状态
    public float attackTime;// 钻地到出现的时间
    public float attackCoolDown;
    public bool isOnGround => groundCheck.isChecked;
    internal AnythingCheck groundCheck;
    public AudioSource drillInAudio;
    public AudioSource drillOutAudio;
    public GameObject aim;
}

public class BorerManFSM : EnemyFSM
{
    public BorerManParameters param;
    public IState currentState;
    public Dictionary<BorerManStateType, IState> state = new Dictionary<BorerManStateType, IState>();
    Dictionary<BorerManStateType, Action> enterStateActions = new Dictionary<BorerManStateType, Action>();

    public override void Awake()
    {
        base.Awake();
        param.groundCheck = GetComponent<AnythingCheck>();
    }

    public override void Start()
    {
        base.Start();
        state = Enum.GetValues(typeof(BorerManStateType)).Cast<BorerManStateType>().ToDictionary
        (
            stateType => stateType,
            stateType => CreateState(stateType)
        );

        enterStateActions = Enum.GetValues(typeof(BorerManStateType)).Cast<BorerManStateType>().ToDictionary
        (
            stateType => stateType,
            _ => (Action)null
        );

        ChangeState(BorerManStateType.Idle);
        GetComponent<SwallowedEnemy>().onLoadActions += () => ChangeState(BorerManStateType.UnderSwallowed);
        GetComponent<SwallowedEnemy>().onBreakActions += () => ChangeState(BorerManStateType.Idle);
        GetComponent<KnockedBackEnemy>().onKnockedBackActions += () => ChangeState(BorerManStateType.KnockedBack);
        GetComponentInChildren<EnemyAttackAnything>().onAttacked += (other) => { if (other.TryGetComponent<SmallBubble>(out var s)) s.isBeingDestroyed = true; BubbleQueue.DestroyBubble(other.gameObject); };

    }

    void Update()
    {
        currentState.OnUpdate();
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public void ChangeState(BorerManStateType stateType, Action onEnter = null)
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

    IState CreateState(BorerManStateType stateType)
    {
        switch (stateType)
        {
            case BorerManStateType.Idle: return new BorerManIdleState(this);
            case BorerManStateType.Attack: return new BorerManAttackState(this);
            case BorerManStateType.KnockedBack: return new BorerManKnockedBackState(this);
            case BorerManStateType.UnderSwallowed: return new BorerManUnderSwallowedState(this);
            default: throw new ArgumentException($"Unknown state type: {stateType}");
        }
    }

    public void OnEnter(BorerManStateType stateType) => enterStateActions[stateType]?.Invoke();
}
