using System;
using UnityEngine;

public class Sickle : MonoBehaviour
{
    public float initSpeed;
    public float flyTime;
    Vector2 direction;
    GameObject father;
    Tween tween;
    Rigidbody2D rb;
    Action onStart;
    bool isReturn;

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
            BubbleQueue.DestroyBubble(other.gameObject);
        }

        if (other.gameObject == father && isReturn)
        {
            Destroy(gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Instantiate(gameObject, transform.position, Quaternion.identity);
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
        tween.AddTween("attack", (x) => rb.linearVelocity = x, direction * initSpeed, Vector2.zero, flyTime,
            Tween.TransitionType.CIRC, Tween.EaseType.OUT);
        tween.AddTween("attack", (x) => isReturn = true, 0, 0, 0);
        tween.AddTween("attack", (x) => rb.linearVelocity = x, Vector2.zero, -direction * initSpeed, flyTime,
            Tween.TransitionType.QUART, Tween.EaseType.IN).Play();
    };

    void OnDestroy()
    {
        father.GetComponent<FarmerFSM>().param.currentSickle = null;
    }



}