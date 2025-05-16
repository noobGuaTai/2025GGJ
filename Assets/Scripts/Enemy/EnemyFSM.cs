using System;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public Vector2 initPos;
    public float health;
    public LayerMask deadlyLayers;
    public AudioSource dieAudio;
    public AudioSource attackAudio;
    public enum EnemySomatoType
    {
        Light,
        Heavy
    }
    public EnemySomatoType somatoType = EnemySomatoType.Light;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public virtual void Start()
    {

    }

    public virtual void ResetSelf()
    {

    }

    public virtual Coroutine ReturnToInitPos(float speed, bool isChangeScale = true)
    {
        return StartCoroutine(MoveToTarget(initPos, speed, isChangeScale: isChangeScale));
    }

    public virtual Coroutine TwoPointPatrol(Vector2 first, Vector2 second, float speed, bool isChangeScale = true)
    {
        (Vector2 adjustedFirst, Vector2 adjustedSecond) = AdjustPatrolPoints(first, second);
        return StartCoroutine(TwoPointPatrolCoroutine(adjustedFirst, adjustedSecond, speed, isChangeScale));
    }

    public virtual Coroutine RandomRangePatrol(Vector2 first, Vector2 second, float speed, float minDistance, bool isChangeScale = true)
    {
        (Vector2 adjustedFirst, Vector2 adjustedSecond) = AdjustPatrolPoints(first, second);
        return StartCoroutine(RandomRangePatrolCoroutine(adjustedFirst, adjustedSecond, speed, minDistance, isChangeScale));
    }

    /// <summary>
    /// 统一处理巡逻点的调整，包含墙壁检测和距离补偿
    /// </summary>
    /// <param name="first">第一个巡逻点</param>
    /// <param name="second">第二个巡逻点</param>
    /// <returns>调整后的两个巡逻点</returns>
    protected virtual (Vector2, Vector2) AdjustPatrolPoints(Vector2 first, Vector2 second)
    {
        const float wallSafeDistance = 14f;
        const float minPointDistance = 0.01f;
        const int maxIterations = 3;

        Vector2 adjustedFirst = AdjustPointForWalls(first, wallSafeDistance);
        Vector2 adjustedSecond = AdjustPointForWalls(second, wallSafeDistance);

        float originalDistance = Vector2.Distance(first, second);
        float adjustedDistance = Vector2.Distance(adjustedFirst, adjustedSecond);

        for (int i = 0; i < maxIterations && adjustedDistance < originalDistance; i++)
        {
            Vector2 direction = (adjustedSecond - adjustedFirst).normalized;
            float delta = originalDistance - adjustedDistance;

            // 尝试延长第二个点
            Vector2 newSecondTarget = adjustedSecond + direction * delta;
            Vector2 newSecondAdjusted = AdjustPointForWalls(newSecondTarget, wallSafeDistance);

            // 如果调整有效
            if (Vector2.Distance(newSecondAdjusted, adjustedSecond) > minPointDistance)
            {
                adjustedSecond = newSecondAdjusted;
                adjustedDistance = Vector2.Distance(adjustedFirst, adjustedSecond);

                // 如果已经足够接近原始距离，则停止调整
                if (adjustedDistance >= originalDistance * 0.95f)
                    break;
            }
            else
            {
                // 尝试反向调整第一个点
                Vector2 newFirstTarget = adjustedFirst - direction * delta;
                Vector2 newFirstAdjusted = AdjustPointForWalls(newFirstTarget, wallSafeDistance);

                if (Vector2.Distance(newFirstAdjusted, adjustedFirst) > minPointDistance)
                {
                    adjustedFirst = newFirstAdjusted;
                    adjustedDistance = Vector2.Distance(adjustedFirst, adjustedSecond);

                    if (adjustedDistance >= originalDistance * 0.95f)
                        break;
                }
                else
                {
                    break;
                }
            }
        }

        return (adjustedFirst, adjustedSecond);
    }

    /// <summary>
    /// 针对墙壁检测调整单个点
    /// </summary>
    /// <param name="targetPoint">目标点</param>
    /// <param name="safeDistance">与墙壁保持的安全距离</param>
    /// <returns>墙壁检测后调整的点</returns>
    protected virtual Vector2 AdjustPointForWalls(Vector2 targetPoint, float safeDistance)
    {
        Vector2 currentPos = transform.position;
        Vector2 direction = (targetPoint - currentPos).normalized;
        float distance = Vector2.Distance(currentPos, targetPoint);

        RaycastHit2D hit = Physics2D.Raycast(currentPos, direction, distance, LayerMask.GetMask("Wall"));

        if (hit.collider != null)
        {
            float adjustedDistance = hit.distance - safeDistance;
            adjustedDistance = Mathf.Max(adjustedDistance, 0.1f);
            return currentPos + direction * adjustedDistance;
        }

        return targetPoint;
    }


    IEnumerator TwoPointPatrolCoroutine(Vector2 first, Vector2 second, float speed, bool isChangeScale = true)
    {
        while (true)
        {
            yield return MoveToTarget(second, speed, isChangeScale: isChangeScale);
            yield return MoveToTarget(first, speed, isChangeScale: isChangeScale);
        }
    }

    IEnumerator RandomRangePatrolCoroutine(Vector2 first, Vector2 second, float speed, float minDistance, bool isChangeScale = true)
    {
        while (true)
        {
            yield return MoveToTarget(new Vector2(UnityEngine.Random.Range(transform.position.x + minDistance, second.x), second.y), speed, isChangeScale: isChangeScale);
            yield return MoveToTarget(new Vector2(UnityEngine.Random.Range(first.x, transform.position.x - minDistance), first.y), speed, isChangeScale: isChangeScale);
        }
    }

    IEnumerator MoveToTarget(Vector2 target, float speed, Action onComplete = null, bool isChangeScale = true)
    {
        float tolerance = 1f;

        while (Mathf.Abs(transform.position.x - target.x) > tolerance)
        {
            float direction = (target.x - transform.position.x) > 0 ? 1 : -1;
            float targetVelocity = direction * speed;
            float velocityChange = targetVelocity - rb.linearVelocityX;
            rb.AddForce(velocityChange * Vector2.right, ForceMode2D.Impulse);

            if (isChangeScale)
                transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);

            yield return null;
        }

        while (Mathf.Abs(rb.linearVelocityX) > 0.1f)
        {
            rb.AddForce(-rb.linearVelocityX * Vector2.right, ForceMode2D.Impulse);
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        transform.position = new Vector3(target.x, transform.position.y, transform.position.z);
        onComplete?.Invoke();
        yield return null;
    }

    /// <summary>
    /// 往左往右发射射线检测物体
    /// </summary>
    /// <param name="leftRange">射线长度</param>
    /// <param name="layer">目标layer</param>
    /// <param name="detected">返回第一个碰到的物体</param>
    /// <param name="direction">-1仅往左发射，1仅往右发射，0左右都发射</param>
    /// <returns>是否检测到物体</returns>
    public virtual bool IsDetectObjectByLayer(float leftRange, float rightRange, LayerMask layer, out GameObject detected, int direction = 0)
    {
        detected = null;
        if (direction >= 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, rightRange, layer);
            if (hit.collider != null)
            {
                detected = hit.collider.gameObject;
                return true;
            }
        }

        if (direction <= 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x, leftRange, layer);
            if (hit.collider != null)
            {
                detected = hit.collider.gameObject;
                return true;
            }
        }

        return false;
    }

    public virtual bool IsDetectObjectByLayer(float range, LayerMask layer, out GameObject detected, int direction = 0)
    {
        detected = null;
        if (direction >= 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, range, layer);
            if (hit.collider != null)
            {
                detected = hit.collider.gameObject;
                return true;
            }
        }

        if (direction <= 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x, range, layer);
            if (hit.collider != null)
            {
                detected = hit.collider.gameObject;
                return true;
            }
        }

        return false;
    }

    public virtual bool IsDetectObjectByComponent<T>(float range, out GameObject detected, int direction = 0, LayerMask? customLayer = null) where T : Component
    {
        detected = null;
        LayerMask layer = customLayer ?? Physics2D.AllLayers;
        if (direction >= 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, range, layer);
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<T>(out var _))
                {
                    detected = hit.collider.gameObject;
                    return true;
                }
            }
        }

        if (direction <= 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x, range, layer);
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<T>(out var _))
                {
                    detected = hit.collider.gameObject;
                    return true;
                }
            }
        }

        return false;
    }



    public virtual void ChaseObject(float speed, GameObject aim, bool isChangeScale = true)
    {
        float tolerance = 1f;
        if (Mathf.Abs(transform.position.x - aim.transform.position.x) > tolerance)
        {
            float direction = (aim.transform.position.x - transform.position.x) > 0 ? 1 : -1;
            float targetVelocity = direction * speed;
            float velocityChange = targetVelocity - rb.linearVelocityX;
            rb.AddForce(velocityChange * Vector2.right, ForceMode2D.Impulse);
            if (isChangeScale)
                transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            rb.AddForce(-rb.linearVelocityX * Vector2.right, ForceMode2D.Impulse);
        }
    }

    public virtual void ChasePosition(float speed, Vector3 aim, bool isChangeScale = true)
    {
        float tolerance = 1f;
        if (Mathf.Abs(transform.position.x - aim.x) > tolerance)
        {
            float direction = (aim.x - transform.position.x) > 0 ? 1 : -1;
            float targetVelocity = direction * speed;
            float velocityChange = targetVelocity - rb.linearVelocityX;
            rb.AddForce(velocityChange * Vector2.right, ForceMode2D.Impulse);
            if (isChangeScale)
                transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            rb.AddForce(-rb.linearVelocityX * Vector2.right, ForceMode2D.Impulse);
        }
    }

    public virtual void InertialChaseObject(float speed, GameObject aim, float inertiaFactor = 0.1f)
    {
        float tolerance = 5f;

        if (Mathf.Abs(transform.position.x - aim.transform.position.x) > tolerance)
        {
            float direction = (aim.transform.position.x - transform.position.x) > 0 ? 1 : -1;
            float targetVelocity = direction * speed;
            float velocityChange = (targetVelocity - rb.linearVelocityX) * inertiaFactor;
            rb.AddForce(velocityChange * Vector2.right, ForceMode2D.Impulse);
        }
        else
        {
            float velocityChange = -rb.linearVelocityX * inertiaFactor;
            rb.AddForce(velocityChange * Vector2.right, ForceMode2D.Impulse);
        }
    }

    public virtual void Die()
    {
        if (dieAudio != null) dieAudio.Play();
        animator.Play("die", 0, 0);
        enabled = false;
        StopAllCoroutines();
        GetComponentsInChildren<Collider2D>().Any(c => c.enabled = false);
        if (TryGetComponent<AggressiveEnemy>(out var a)) Destroy(a);
        Destroy(gameObject, 1f);
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & deadlyLayers) != 0)
        {
            Die();
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (((1 << other.gameObject.layer) & deadlyLayers) != 0)
        {
            Die();
        }
    }

}