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


    [Header("Player Components")]
    public Rigidbody2D rb;
    public Camera mainCamera;
    public Animator animator;
    public SpriteRenderer sr;
    [Tooltip("Sprites used during walking animations.")]
    public Sprite[] walkSprites;

    [Header("Audio Settings")]
    public AudioSource dieAudio;
    public AudioSource winAudio;
    public AudioSource walkAudio;
    public AudioSource blowAudio;
    public AudioSource shootAudio;
    public AudioSource coinAudio;
    public AudioSource underAttackAudio;
    internal bool fireInput;
    public float shootTimer;
    public float shootCooldown;
    public float moveSpeed = 10;
    public int health;
    public float jumpForce = 100f;
    public GameObject bubble;
    internal bool blowInput;
    public Collider2D blowArea;
    internal float blowPressStartTime = 0f;
    public bool isBlowing = false;
    public GameObject existingBubble;
    public Animator bubblingAnimator;

    internal float initGravityScale = 50;
}


public class PlayerFSM : MonoSingleton<PlayerFSM>
{
    public PlayerParameters parameters;
    public IState currentState;
    public Dictionary<PlayerStateType, IState> state = new Dictionary<PlayerStateType, IState>();

    private Tween tween;

    public override void Init()
    {
        parameters.rb = GetComponent<Rigidbody2D>();
        parameters.animator = GetComponentInChildren<Animator>();
        parameters.sr = GetComponent<SpriteRenderer>();
        parameters.initGravityScale = parameters.rb.gravityScale;
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

        if (parameters.health <= 0)
        {
            // Die();
        }
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
        Mathf.Clamp(parameters.shootTimer -= Time.fixedDeltaTime, 0, float.MaxValue);
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
        if (!parameters.dieAudio.isPlaying)
            parameters.dieAudio.Play();
        parameters.rb.linearVelocity = Vector2.zero;
        ChangeState(PlayerStateType.Idle);
        transform.rotation = Quaternion.Euler(0, 0, -90 * transform.localScale.x);
        enabled = false;
        GetComponent<Collider2D>().enabled = false;
        parameters.rb.gravityScale = 0;
        UIManager.Instance.CancelInvoke("CloseDialog");
        Invoke("Restore", 1f);
    }

    void Restore()
    {
        GameManager.Instance.ResetGame();
        enabled = true;
        GetComponent<Collider2D>().enabled = true;
        parameters.rb.gravityScale = parameters.initGravityScale;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        UIManager.Instance.ShowDialog($"enemy{GameManager.Instance.level}");
        print(1);
    }

    public void PlayerMove(InputAction.CallbackContext context)
    {
        parameters.moveInput = context.ReadValue<Vector2>();
        // parameters.walkAudio.Play();

    }

    public void PlayerFire(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.level == 0)
            return;
        parameters.fireInput = context.ReadValueAsButton();
        Fire();
    }


    void Fire()
    {
        if (parameters.fireInput && parameters.shootTimer <= 0f && parameters.existingBubble == null)
        {
            parameters.bubblingAnimator.Play("bubbling");
            Invoke("InstantiateBubble", 0.5f);
            parameters.shootTimer = parameters.shootCooldown;
        }
        else
        {
            parameters.fireInput = false;
        }
    }

    public void Playerblow(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            parameters.isBlowing = true;
            parameters.blowPressStartTime = Time.time;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            if (parameters.isBlowing)
            {
                float pressDuration = Math.Clamp(Time.time - parameters.blowPressStartTime, 0, 1);
                parameters.isBlowing = false;
                blow(pressDuration);
            }
        }
    }


    void blow(float duration)
    {
        parameters.blowArea.GetComponent<Blow>().blowForce = duration * 1000f;
        parameters.blowArea.gameObject.SetActive(true);
    }


    void InstantiateBubble()
    {
        if (!parameters.shootAudio.isPlaying)
            parameters.shootAudio.Play();
        var b = Instantiate(parameters.bubble, transform.position + Vector3.left * transform.localScale.x * 23f, Quaternion.identity);
        // b.transform.localScale = new Vector3(25, 25, 25);
        // b.transform.SetParent(transform.Find("/Root/Bubbles"), false);
        b.GetComponent<Bubble>().bubbleState = Bubble.BubbleState.hard;
        parameters.existingBubble = b;
    }

    public void BubbleBomb(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && parameters.existingBubble != null)
        {
            parameters.existingBubble.GetComponent<Bubble>().Break();
            parameters.existingBubble = null;
        }
    }



}