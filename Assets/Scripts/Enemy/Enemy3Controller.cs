using Unity.Mathematics;
using UnityEngine;

public class Enemy3Controller : EnemyController
{
    public Collider2D detect;
    public float moveSpeed = 100f;
    private Rigidbody2D rb;
    public GameObject player;
    public Vector2 initPos;
    public Key key;
    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initPos = transform.position;
    }



    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            rb.linearVelocityX = Mathf.Sign(player.transform.position.x - transform.position.x) * moveSpeed;
            transform.localScale = new Vector3(Mathf.Sign(transform.position.x - player.transform.position.x), 1, 1);
            animator.Play("run");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            rb.linearVelocityX = 0;
            animator.Play("idle");
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Nail") || other.gameObject.layer == LayerMask.NameToLayer("Stone"))
        {
            key.gameObject.SetActive(true);
        }
    }

    public override void ResetSelf()
    {
        transform.position = initPos;
    }
}
