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


    // void Swallow(GameObject g)
    // {
    //     g.transform.SetParent(transform, false);
    // }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<SwallowedObject>() != null)
        {
            other.gameObject.GetComponent<SwallowedObject>().OnLoad(this);
        }
    }
}
