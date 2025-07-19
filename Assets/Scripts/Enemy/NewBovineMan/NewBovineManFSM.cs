using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum NewBovineManStateType
{
    Idle,
    Charge,
    Attack,
    UnderSwallowed,
    KnockedBack,
    Return
}

[Serializable]
public class NewBovineManParameters
{
    public NewBovineManStateType currentState;
    public float detectRange;
    public float SprintSpeed;
    public float ChargeTime;
    public float pushForce;
    public float returnSpeed;
    public float calmDownTime;
    public Color initColor;
    public bool isOnGround => groundCheck.isChecked;
    internal AnythingCheck groundCheck;
    internal Tween tween;
}

public class NewBovineManFSM : EnemyFSM
{
    public NewBovineManParameters param;
    public IState currentState;
    public Dictionary<NewBovineManStateType, IState> state = new Dictionary<NewBovineManStateType, IState>();
    Dictionary<NewBovineManStateType, Action> enterStateActions = new Dictionary<NewBovineManStateType, Action>();

    public override void Awake()
    {
        base.Awake();
        param.groundCheck = GetComponentInChildren<AnythingCheck>();
        param.tween = gameObject.AddComponent<Tween>();
    }

    public override void Start()
    {
        base.Start();
        state = Enum.GetValues(typeof(NewBovineManStateType)).Cast<NewBovineManStateType>().ToDictionary
        (
            stateType => stateType,
            stateType => CreateState(stateType)
        );

        enterStateActions = Enum.GetValues(typeof(NewBovineManStateType)).Cast<NewBovineManStateType>().ToDictionary
        (
            stateType => stateType,
            _ => (Action)null
        );
        param.initColor = GetComponent<SpriteRenderer>().color;

        ChangeState(NewBovineManStateType.Idle);
        GetComponent<SwallowedEnemy>().onLoadActions += () => ChangeState(NewBovineManStateType.UnderSwallowed);
        GetComponent<SwallowedEnemy>().onBreakActions += () => ChangeState(NewBovineManStateType.Idle);
        GetComponent<KnockedBackEnemy>().onKnockedBackActions += () => ChangeState(NewBovineManStateType.KnockedBack);
    }

    void Update()
    {
        currentState.OnUpdate();
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public void ChangeState(NewBovineManStateType stateType, Action onEnter = null)
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

    IState CreateState(NewBovineManStateType stateType)
    {
        switch (stateType)
        {
            case NewBovineManStateType.Idle: return new NewBovineManIdleState(this);
            case NewBovineManStateType.Charge: return new NewBovineManChargeState(this);
            case NewBovineManStateType.Attack: return new NewBovineManAttackState(this);
            case NewBovineManStateType.UnderSwallowed: return new NewBovineManUnderSwallowedState(this);
            case NewBovineManStateType.KnockedBack: return new NewBovineManKnockedBackState(this);
            case NewBovineManStateType.Return: return new NewBovineManReturnState(this);
            default: throw new ArgumentException($"Unknown state type: {stateType}");
        }
    }

    public void OnEnter(NewBovineManStateType stateType) => enterStateActions[stateType]?.Invoke();
    public void CalmDown() => GetComponent<SpriteRenderer>().color = param.initColor;
    public void ToAttack() => ChangeState(NewBovineManStateType.Attack);

    public override void OnTriggerEnter2D(Collider2D other)
    {

        base.OnTriggerEnter2D(other);
        StartCoroutine(HandleKnockBack(other));
    }

    IEnumerator HandleKnockBack(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (currentState is NewBovineManAttackState)
            {
                ChangeState(NewBovineManStateType.UnderSwallowed);
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.localScale.x * param.pushForce, param.pushForce * 0.8f), ForceMode2D.Impulse);
                CalmDown();
                yield return new WaitForSeconds(0.5f);
                ChangeState(NewBovineManStateType.Return);
            }
        }
    }
}
