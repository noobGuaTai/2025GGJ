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

        if (other.gameObject == father)
        {
            Destroy(gameObject);
        }

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
        Vector2 directionToTarget = (Vector2)aim - (Vector2)transform.position;

        float dx = directionToTarget.x;
        float dy = directionToTarget.y;

        float g = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);

        float m = rb.mass;
        float v0y = high / m;

        float force_x;

        if (Mathf.Approximately(dy, 0))
        {
            force_x = dx * g * m / (2 * v0y);
        }
        else
        {
            float discriminant = v0y * v0y - 2 * g * dy;

            if (discriminant < 0)
            {
                Debug.LogWarning("Target is too high to reach with the given vertical force.");
                force_x = directionToTarget.normalized.x * high;
            }
            else
            {
                float t1 = (v0y + Mathf.Sqrt(discriminant)) / g;
                float t2 = (v0y - Mathf.Sqrt(discriminant)) / g;

                float timeToTarget;
                if (t1 > 0 && t2 > 0)
                {
                    timeToTarget = (dy > 0) ? Mathf.Min(t1, t2) : Mathf.Max(t1, t2);
                }
                else if (t1 > 0)
                {
                    timeToTarget = t1;
                }
                else if (t2 > 0)
                {
                    timeToTarget = t2;
                }
                else
                {
                    timeToTarget = v0y / g;
                }

                float v0x = dx / timeToTarget;
                force_x = m * v0x;
            }
        }

        rb.AddForce(new Vector2(force_x, high), ForceMode2D.Impulse);
    };

    void OnDestroy()
    {

    }



}