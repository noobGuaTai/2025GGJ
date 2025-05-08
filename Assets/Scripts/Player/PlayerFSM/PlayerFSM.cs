using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum PlayerStateType
{
    Idle,
    Move,
    Jump,
    KnockedBack,
    Rebound
}


[Serializable]
public class PlayerParameters
{
    public PlayerStateType currentState;
    public Vector2 moveInput;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer sr;
    public Sprite[] walkSprites;
    public bool fireInput;
    public GameObject bubblePrefab;
    public GameObject weaponCoinPrefab;
    public bool blowInput;
    public Collider2D blowArea;
    public Animator bubblingAnimator;
    public bool jumpInput;
    public bool isOnGround => groundCheck.isChecked;
    public bool isOnBigBubble => bubbleCheck.isChecked;
    internal AnythingCheck groundCheck;
    internal BubbleCheck bubbleCheck;
    internal PlayerInventory playerInventory;
}

[Serializable]
public class PlayerDelegateParameters
{
    public Action onDie;
    public Action onBlowBubble;
    public Action onRebound;
}

[Serializable]
public class PlayerAttributes
{
    internal float blowTimer;
    public float blowCooldown;
    public float moveSpeed = 10;
    public int health;
    public float jumpSpeed = 100f;
    internal float blowPressStartTime = 0f;
    public bool isBlowing = false;
    internal float initGravityScale = 50;
    public float throwCoinSpeed;
    public float pushCooldown;
    internal float pushTimer;
}


public class PlayerFSM : MonoSingleton<PlayerFSM>
{
    public PlayerParameters param;
    public PlayerDelegateParameters delegateParam;
    public PlayerAttributes attributes;
    public IState currentState;
    public Dictionary<PlayerStateType, IState> state = new Dictionary<PlayerStateType, IState>();

    private Tween tween;

    public override void Init()
    {
        param.rb = GetComponent<Rigidbody2D>();
        param.animator = GetComponentInChildren<Animator>();
        param.sr = GetComponent<SpriteRenderer>();
        param.playerInventory = GetComponent<PlayerInventory>();
        attributes.initGravityScale = param.rb.gravityScale;
        Addressables.LoadAssetAsync<GameObject>("Assets/Prefab/Bubble/SmallBubble.prefab").Completed += (AsyncOperationHandle<GameObject> handle) => { if (handle.Status == AsyncOperationStatus.Succeeded) param.bubblePrefab = handle.Result; };
        Addressables.LoadAssetAsync<GameObject>("Assets/Prefab/Item/WeaponCoin.prefab").Completed += (AsyncOperationHandle<GameObject> handle) => { if (handle.Status == AsyncOperationStatus.Succeeded) param.weaponCoinPrefab = handle.Result; };
        InitAction();
    }


    void Start()
    {
        state.Add(PlayerStateType.Idle, new PlayerIdleState(this));
        state.Add(PlayerStateType.Move, new PlayerMoveState(this));
        state.Add(PlayerStateType.Jump, new PlayerJumpState(this));
        state.Add(PlayerStateType.KnockedBack, new PlayerKnockedBackState(this));
        state.Add(PlayerStateType.Rebound, new PlayerReboundState(this));
        tween = GetComponent<Tween>();
        ChangeState(PlayerStateType.Idle);

        param.groundCheck = GetComponent<AnythingCheck>();
        param.bubbleCheck = GetComponent<BubbleCheck>();
        enabled = false;
    }

    void InitAction()
    {
        var playerInput = GetComponent<PlayerInput>();
        Dictionary<string, Action<InputAction.CallbackContext>> actionMap = new()
        {
            { "Move", PlayerMove },
            { "Blow", PlayerBlowBubble },
            { "Push", PlayerPush },
            { "Bomb", BubbleBomb },
            { "Jump", PlayerJump },
            { "Throw", PlayerThrowCoin }
        };

        foreach (var pair in actionMap)
        {
            InputAction action = playerInput.actions.FindAction(pair.Key);
            action.started += pair.Value;
            action.performed += pair.Value;
            action.canceled += pair.Value;
        }
    }

    void Update()
    {
        currentState.OnUpdate();
        attributes.pushTimer += Time.deltaTime;
        if (attributes.health <= 0)
        {
            // Die();
        }
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
        Mathf.Clamp(attributes.blowTimer -= Time.fixedDeltaTime, 0, float.MaxValue);
    }

    public void ChangeState(PlayerStateType stateType)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = state[stateType];
        param.currentState = stateType;
        currentState.OnEnter();
    }

    public void Die()
    {
        param.rb.linearVelocity = Vector2.zero;
        ChangeState(PlayerStateType.Idle);
        transform.rotation = Quaternion.Euler(0, 0, -90 * transform.localScale.x);
        enabled = false;
        GetComponent<Collider2D>().enabled = false;
        param.rb.gravityScale = 0;
        UIManager.Instance.CancelInvoke("CloseDialog");
        Invoke("Restore", 1f);
        delegateParam.onDie?.Invoke();
    }

    void Restore()
    {
        GameManager.Instance.ResetGame();
        enabled = true;
        GetComponent<Collider2D>().enabled = true;
        param.rb.gravityScale = attributes.initGravityScale;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        // UIManager.Instance.ShowDialog($"enemy{GameManager.Instance.level}");
    }

    public void PlayerMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started || context.phase == InputActionPhase.Performed)
        {
            param.moveInput = context.ReadValue<Vector2>();
        }

        else if (context.phase == InputActionPhase.Canceled)
            param.moveInput = Vector2.zero;

    }

    public void PlayerJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            param.jumpInput = context.ReadValueAsButton();
        else if (context.phase == InputActionPhase.Canceled)
            param.jumpInput = false;

    }

    public void PlayerBlowBubble(InputAction.CallbackContext context)
    {
        // if (GameManager.Instance.level == 0)
        //     return;
        param.fireInput = context.ReadValueAsButton();
        BlowBubble();
    }


    void BlowBubble()
    {
        if (attributes.blowTimer <= 0f)
        {
            param.bubblingAnimator.gameObject.transform.localPosition =
                param.moveInput.y > 0 ? new Vector3(0, 30, 0) : param.moveInput.y < 0 ? new Vector3(0, -30, 0) : new Vector3(-23, 0, 0);
            param.bubblingAnimator.Play("bubbling");
            StartCoroutine(InstantiateBubble(param.moveInput.y));
            attributes.blowTimer = attributes.blowCooldown;
        }
        else
        {
            param.fireInput = false;
        }
    }

    public void PlayerLongPush(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            attributes.isBlowing = true;
            attributes.blowPressStartTime = Time.time;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            if (attributes.isBlowing)
            {
                float pressDuration = Math.Clamp(Time.time - attributes.blowPressStartTime, 0, 1);
                attributes.isBlowing = false;
                Push(pressDuration);
            }
        }
    }

    public void PlayerPush(InputAction.CallbackContext context)
    {
        if (attributes.pushTimer <= attributes.pushCooldown)
            return;
        if (context.phase == InputActionPhase.Started)
        {
            if (param.moveInput.y == 0)
            {
                param.blowArea.GetComponent<Blow>().direction = Vector2.left * transform.localScale.x;
                param.animator.Play("push_hori", 0, 0);
            }
            else
            {
                param.blowArea.GetComponent<Blow>().direction = new Vector2(param.moveInput.x, param.moveInput.y).normalized;
                param.animator.Play("push_up", 0, 0);
            }
            Invoke("Push", 0.2f);
            attributes.pushTimer = 0;
        }

    }

    public void PlayerThrowCoin(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && param.playerInventory.coins > 0)
        {
            var c = Instantiate(param.weaponCoinPrefab, transform.position, Quaternion.identity).GetComponent<WeaponCoin>();
            c.onInit += () => c.rb.linearVelocity = new Vector2(-transform.localScale.x, 2).normalized * attributes.throwCoinSpeed;
            param.playerInventory.coins--;
        }

    }

    void Push()
    {
        param.blowArea.gameObject.SetActive(true);
    }
    void Push(float duration)
    {
        param.blowArea.GetComponent<Blow>().blowForce = duration * 1000f;
        param.blowArea.gameObject.SetActive(true);
    }




    IEnumerator InstantiateBubble(float moveInputY)
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 p = transform.position + (moveInputY > 0 ? new Vector3(0, 30, 0) : moveInputY < 0 ? new Vector3(0, -30, 0) : new Vector3(-23, 0, 0) * transform.localScale.x);
        var b = Instantiate(param.bubblePrefab, p, Quaternion.identity);
        delegateParam.onBlowBubble?.Invoke();
    }

    public void BubbleBomb(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && (BubbleQueue.smallBubbleNums > 0 || BubbleQueue.bigBubbleNums > 0))
        {
            var b = BubbleQueue.Dequeue();
            b.GetComponent<BaseBubble>().Break();
        }
    }

    public void OnKnockedBack()
    {
        ChangeState(PlayerStateType.KnockedBack);
    }

    public void Move()
    {
        Vector2 targetVelocity = new Vector2(param.moveInput.x * attributes.moveSpeed, param.rb.linearVelocity.y);

        Vector2 currentVelocity = param.rb.linearVelocity;
        Vector2 velocityChange = new Vector2(targetVelocity.x - currentVelocity.x, 0);

        param.rb.AddForce(velocityChange, ForceMode2D.Impulse);

        if (Mathf.Abs(param.rb.linearVelocity.x) > attributes.moveSpeed)
        {
            Vector2 clampedVelocity = new Vector2(
                Mathf.Sign(param.rb.linearVelocity.x) * attributes.moveSpeed,
                param.rb.linearVelocity.y
            );
            param.rb.linearVelocity = clampedVelocity;
        }
    }
}