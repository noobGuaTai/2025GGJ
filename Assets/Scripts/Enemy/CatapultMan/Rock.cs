using System;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public float high;
    Vector2 direction;
    Vector3 aim;
    GameObject father;
    Tween tween;
    Rigidbody2D rb;
    Action onStart;

    void Start()
    {
        tween = gameObject.AddComponent<Tween>();
        rb = GetComponent<Rigidbody2D>();
        onStart?.Invoke();
        Destroy(gameObject, 8f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerFSM>().Die();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Bubble"))
        {
            if (other.TryGetComponent<SmallBubble>(out var _))
                BubbleQueue.DestroyBubble(other.gameObject);
        }

        // if (other.gameObject == father)
        // {
        //     Destroy(gameObject);
        // }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
    }
    public void Init(Vector2 direction, GameObject father, Vector2 pos)
    {
        this.direction = direction;
        this.father = father;
        aim = pos;
    }
    public void Attack() => onStart = () =>
    {
        float g = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
        float m = rb.mass;

        Vector2 startPos = transform.position;
        Vector2 targetPos = aim;
        float dx = targetPos.x - startPos.x;
        float dy = targetPos.y - startPos.y;

        float apexY = Mathf.Max(startPos.y, targetPos.y) + high;

        float v0y = Mathf.Sqrt(2 * g * (apexY - startPos.y));

        float tUp = v0y / g;
        float tDown = Mathf.Sqrt(2 * (apexY - targetPos.y) / g);
        float flightTime = tUp + tDown;

        float v0x = dx / flightTime;

        Vector2 impulse = new Vector2(v0x * m, v0y * m);

        rb.AddForce(impulse, ForceMode2D.Impulse);
    };



    void OnDestroy()
    {

    }



}