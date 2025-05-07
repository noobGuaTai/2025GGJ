using System;
using UnityEngine;

public class Tire : MonoBehaviour
{
    public float rollSpeed;
    public float accelerateTime;
    Vector2 direction;
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

    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerFSM>().Die();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Bubble"))
        {
            if (other.gameObject.TryGetComponent<SmallBubble>(out var _))
                BubbleQueue.DestroyBubble(other.gameObject);
        }

        // if (other.gameObject == father)
        // {
        //     Destroy(gameObject);
        // }

        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
    }
    public void Init(Vector2 direction, GameObject father)
    {
        this.direction = direction;
        this.father = father;
    }
    public void Attack() => onStart = () =>
    {
        tween.AddTween("attack", (x) =>
        {
            rb.AddForce(x, ForceMode2D.Force);
        }, Vector2.zero, direction * rollSpeed, accelerateTime).Play();
    };

    void OnDestroy()
    {

    }



}