using System;
using UnityEngine;

public class SwallowedObject : MonoBehaviour
{
    private Collider2D c;
    protected Rigidbody2D rb;
    private float initGravityScale = 50;
    private Transform parent;
    public virtual void Start()
    {
        c = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        initGravityScale = rb.gravityScale;
    }
    public virtual void Update()
    {
        if (parent != null)
        {
            transform.position = parent.transform.position;
        }
    }
    public virtual void OnLoad(BaseBubble bubble)
    {
        parent = bubble.transform;
        // transform.SetParent(bubble.transform, false);// 不知道为什么移动父物体子物体在世界坐标下不动
        // action = () => { };


        c.enabled = false;
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
    }
    public virtual void OnBreak(BaseBubble bubble)
    {
        // transform.SetParent(parent, false);
        parent = null;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0f;
        transform.position = bubble.transform.position;
        c.enabled = true;
        rb.gravityScale = initGravityScale;
    }
}