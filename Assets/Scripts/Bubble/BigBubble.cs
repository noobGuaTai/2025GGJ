using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BigBubble : BaseBubble
{
    public LayerMask explosionEffectMask;
    public Collider2D outsideCollider;
    public Collider2D triggerCollider;
    public float explosionRadius;
    public float explosionForce;
    public float contactGroundThreshold;// 触地超过这个时间就会吸附到地面
    float contactGroundTimer;// 触地计时器
    public bool isAbsorbed;
    public AnimationCurve impactedSpeedCurve;
    Tween tween;
    float radius = 7f;
    HashSet<Collider2D> collidingObjects = new HashSet<Collider2D>();

    public override void Awake()
    {
        base.Awake();
        tween = gameObject.AddComponent<Tween>();
        Invoke("SetGravityScale", 1f);
    }

    public override void Start()
    {
        base.Start();
    }
    public override void OnCollisionEnter2D(Collision2D other)
    {
        if (rb.bodyType != RigidbodyType2D.Kinematic)
            SwallowObject(other.gameObject);
        DestroyBubble(other.gameObject);
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        collidingObjects.Add(other);
    }

    public override void OnTriggerStay2D(Collider2D other)
    {
        base.OnTriggerStay2D(other);
        if ((other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Wall")) && isAbsorbed == false)
        {
            while (true)
            {
                contactGroundTimer += Time.fixedDeltaTime;
                if (contactGroundTimer > contactGroundThreshold)
                {
                    Absorbed();
                    break;
                }
            }
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        collidingObjects.Remove(other);
        if (collidingObjects.Count == 0)
            contactGroundTimer = 0;

    }

    void Absorbed()
    {
        Vector3 contactPoint = CalculateContactPoint();
        if (contactPoint == Vector3.zero)
        {
            contactGroundTimer = 0;
            isAbsorbed = false;
            return;
        }

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        tween.AddTween(x => transform.position = x, transform.position,
         contactPoint + GetNormalVector(contactPoint) * radius,
          0.5f, Tween.TransitionType.QUART, Tween.EaseType.IN).Play();
        isAbsorbed = true;
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
            float forceFactor = impactedSpeedCurve.Evaluate(distance / explosionRadius);
            rb.linearVelocity = direction.normalized * explosionForce * forceFactor;

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

    /// <summary>
    /// 判断接触点的法线方向
    /// </summary>
    /// <param name="contactPointPosition"></param>
    /// <param name="bubblePosition"></param>
    /// <returns></returns>
    Vector3 GetNormalVector(Vector3 contactPointPosition)
    {
        Vector2 direction = contactPointPosition - transform.position;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            return direction.x > 0 ? Vector2.right : Vector2.left;
        else
            return direction.y > 0 ? Vector2.up : Vector2.down;
    }

    /// <summary>
    /// 搜索最近的接触点
    /// </summary>
    /// <returns>接触点</returns>
    Vector2 CalculateContactPoint()
    {
        Vector2 origin = transform.position;
        Vector2[] directions = new Vector2[]
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        float closestDistance = float.MaxValue;
        Vector2 closestPoint = Vector2.zero;
        bool hitDetected = false;
        foreach (Vector2 direction in directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, 30f, LayerMask.GetMask("Ground", "Wall"));
            if (hit.collider != null)
            {
                float distance = Vector2.Distance(origin, hit.point);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = hit.point;
                    hitDetected = true;
                }

            }
        }
        return hitDetected ? closestPoint : Vector2.zero;
    }

    public override void SwallowObject(GameObject other)
    {
        if (other.TryGetComponent<EnemyFSM>(out var e))
            if (e.somatoType == EnemyFSM.EnemySomatoType.Light)
                BubbleQueue.DestroyBubble(gameObject);
            else
            {
                base.SwallowObject(other);
                outsideCollider.excludeLayers = 0;
                triggerCollider.enabled = false;
            }

    }
}
