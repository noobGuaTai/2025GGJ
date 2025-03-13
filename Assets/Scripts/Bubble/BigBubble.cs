using System.Collections;
using UnityEngine;


public class BigBubble : BaseBubble
{
    public LayerMask explosionEffectMask;
    public float explosionRadius;
    public float explosionForce;
    public float contactGroundThreshold;// 触地超过这个时间就会吸附到地面
    float contactGroundTimer;// 触地计时器
    Coroutine contactGroundTimeCoroutine;
    Tween tween;
    float radius = 7.5f;

    public override void Start()
    {
        base.Start();
        tween = gameObject.AddComponent<Tween>();
        Invoke("SetGravityScale", 1f);
    }
    public override void OnCollisionEnter2D(Collision2D other)
    {
        // if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        // {
        //     rb.bodyType = RigidbodyType2D.Kinematic;
        //     rb.linearVelocity = Vector2.zero;
        // }
        if (rb.bodyType != RigidbodyType2D.Kinematic)
            SwallowObject(other.gameObject);
        DestroyBubble(other.gameObject);
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if ((other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Wall")) && contactGroundTimeCoroutine == null)
        {
            contactGroundTimeCoroutine = StartCoroutine(CumulativeContactGroundTime(other));
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (contactGroundTimeCoroutine != null)
        {
            StopCoroutine(contactGroundTimeCoroutine);
            contactGroundTimer = 0;
        }
    }

    void SetGravityScale() => rb.gravityScale = 1;

    public override void Break()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionEffectMask);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb == null || rb.bodyType != RigidbodyType2D.Dynamic)
                continue;

            Vector2 direction = (Vector2)hit.transform.position - (Vector2)transform.position;
            float distance = direction.magnitude;
            float forceFactor = 1f - (distance / explosionRadius);
            rb.AddForce(direction.normalized * explosionForce * forceFactor, ForceMode2D.Impulse);

            if (hit.TryGetComponent<KnockedBackObject>(out var k))
            {
                k.OnKnockedBack();
            }
        }
        base.Break();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    IEnumerator CumulativeContactGroundTime(Collider2D other)
    {
        Vector2 contactPoint = CalculateContactPoint(other);

        while (true)
        {
            contactGroundTimer += Time.deltaTime;
            if (contactGroundTimer > contactGroundThreshold)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
                tween.AddTween(x => transform.position = x, transform.position,
                 transform.position + GetCardinalDirection(contactPoint, transform.position) * radius,
                  2f, Tween.TransitionType.QUART, Tween.EaseType.IN_OUT).Play();
                yield break;
            }
            yield return null;
        }
    }

    Vector3 GetCardinalDirection(Vector2 contactPointPosition, Vector2 bubblePosition)
    {
        Vector2 direction = contactPointPosition - bubblePosition;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            return direction.x > 0 ? Vector2.right : Vector2.left;
        else
            return direction.y > 0 ? Vector2.up : Vector2.down;
    }

    Vector2 CalculateContactPoint(Collider2D other)
    {
        // 获取两个碰撞体的边界
        Bounds myBounds = GetComponent<Collider2D>().bounds;
        Bounds otherBounds = other.bounds;

        // 计算边界的重叠区域
        Bounds intersection = new Bounds();
        intersection.min = new Vector3(
            Mathf.Max(myBounds.min.x, otherBounds.min.x),
            Mathf.Max(myBounds.min.y, otherBounds.min.y),
            0
        );
        intersection.max = new Vector3(
            Mathf.Min(myBounds.max.x, otherBounds.max.x),
            Mathf.Min(myBounds.max.y, otherBounds.max.y),
            0
        );

        // 返回重叠区域的中心作为近似接触点
        return intersection.center;
    }
}
