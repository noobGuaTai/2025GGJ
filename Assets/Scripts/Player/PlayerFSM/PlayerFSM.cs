using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerStateType
{
    Idle,
    Move,
    Jump
}


[Serializable]
public class PlayerParameters
{
    public Vector2 moveInput;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer sr;
    public Sprite[] walkSprites;
    public bool fireInput;
    public GameObject bubblePrefab;
    public bool blowInput;
    public Collider2D blowArea;
    public GameObject existingBubble;
    public Animator bubblingAnimator;
    public bool jumpInput;
    public bool isOnGround;
}

[Serializable]
public class PlayerDelegateParameters
{
    public Action onDie;
    public Action onShoot;
}

[Serializable]
public class PlayerAttributes
{
    public float shootTimer;
    public float shootCooldown;
    public float moveSpeed = 10;
    public int health;
    public float jumpForce = 100f;
    internal float blowPressStartTime = 0f;
    public bool isBlowing = false;
    internal float initGravityScale = 50;
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
        attributes.initGravityScale = param.rb.gravityScale;
    }


    void Start()
    {
        state.Add(PlayerStateType.Idle, new PlayerIdleState(this));
        state.Add(PlayerStateType.Move, new PlayerMoveState(this));
        state.Add(PlayerStateType.Jump, new PlayerJumpState(this));
        tween = GetComponent<Tween>();
        ChangeState(PlayerStateType.Idle);

    }

    void Update()
    {
        currentState.OnUpdate();

        if (attributes.health <= 0)
        {
            // Die();
        }
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
        Mathf.Clamp(attributes.shootTimer -= Time.fixedDeltaTime, 0, float.MaxValue);
    }

    public void ChangeState(PlayerStateType stateType)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = state[stateType];
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
        UIManager.Instance.ShowDialog($"enemy{GameManager.Instance.level}");
    }

    public void PlayerMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started || context.phase == InputActionPhase.Performed)
            param.moveInput = context.ReadValue<Vector2>();
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

    public void PlayerFire(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.level == 0)
            return;
        param.fireInput = context.ReadValueAsButton();
        Fire();
    }


    void Fire()
    {
        if (param.fireInput && attributes.shootTimer <= 0f && param.existingBubble == null)
        {
            param.bubblingAnimator.Play("bubbling");
            Invoke("InstantiateBubble", 0.5f);
            attributes.shootTimer = attributes.shootCooldown;
        }
        else
        {
            param.fireInput = false;
        }
    }

    public void PlayerLongblow(InputAction.CallbackContext context)
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
                blow(pressDuration);
            }
        }
    }

    public void Playerblow(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (param.moveInput == Vector2.zero)
            {
                param.blowArea.GetComponent<Blow>().direction = Vector2.left * transform.localScale.x;
            }
            else
            {
                param.blowArea.GetComponent<Blow>().direction = new Vector2(param.moveInput.x, param.moveInput.y).normalized;
            }
            blow();
        }

    }


    void blow(float duration)
    {
        param.blowArea.GetComponent<Blow>().blowForce = duration * 1000f;
        param.blowArea.gameObject.SetActive(true);
    }

    void blow()
    {
        param.blowArea.gameObject.SetActive(true);
    }


    void InstantiateBubble()
    {
        var b = Instantiate(param.bubblePrefab, transform.position + Vector3.left * transform.localScale.x * 23f, Quaternion.identity);
        // b.transform.localScale = new Vector3(25, 25, 25);
        // b.transform.SetParent(transform.Find("/Root/Bubbles"), false);
        b.GetComponent<Bubble>().bubbleState = Bubble.BubbleState.hard;
        param.existingBubble = b;
        delegateParam.onShoot?.Invoke();
    }

    public void BubbleBomb(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && param.existingBubble != null)
        {
            param.existingBubble.GetComponent<Bubble>().Break();
            param.existingBubble = null;
        }
    }



}