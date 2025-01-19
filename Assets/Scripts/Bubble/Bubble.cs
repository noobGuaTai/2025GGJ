using UnityEngine;


public class Bubble : MonoBehaviour
{
    public enum BubbleState
    {
        soft,
        hard
    }
    public Animator animator;
    public Rigidbody2D rb;
    public BubbleState bubbleState = BubbleState.soft;
    public Collider2D colliders;
    public GameObject swallowedObject;
    public bool isDestoryOnGround = false;
    public AudioSource destory;
    void Awake()
    {
        colliders = GetComponent<Collider2D>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        colliders.isTrigger = bubbleState == BubbleState.soft ? true : false;
    }


    void Update()
    {

    }

    public void Break()
    {
        if (swallowedObject != null)
            swallowedObject?.GetComponent<SwallowedObject>().OnBreak(this);
        swallowedObject = null;
        animator.Play("bomb");
        colliders.enabled = false;
        destory.Play();
        Destroy(gameObject, 0.4f);
    }


    // void Swallow(GameObject g)
    // {
    //     g.transform.SetParent(transform, false);
    // }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<SwallowedObject>() != null && swallowedObject == null)
        {
            other.gameObject.GetComponent<SwallowedObject>().OnLoad(this);
            swallowedObject = other.gameObject;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Nail") || (other.gameObject.layer == LayerMask.NameToLayer("Ground") && isDestoryOnGround))
        {
            Break();
        }
    }
}
