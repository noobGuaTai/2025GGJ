using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public Vector2 initPos;
    public float health;
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

    public virtual Coroutine ReturnToInitPos(float speed)
    {
        return StartCoroutine(MoveToTarget(initPos, speed));
    }

    public virtual Coroutine TwoPointPatrol(Vector2 first, Vector2 second, float speed)
    {
        (Vector2 adjustedFirst, Vector2 adjustedSecond) = AdjustPatrolPoints(first, second);
        return StartCoroutine(TwoPointPatrolCoroutine(adjustedFirst, adjustedSecond, speed));
    }

    public virtual Coroutine RandomRangePatrol(Vector2 first, Vector2 second, float speed, float minDistance)
    {
        (Vector2 adjustedFirst, Vector2 adjustedSecond) = AdjustPatrolPoints(first, second);
        return StartCoroutine(RandomRangePatrolCoroutine(adjustedFirst, adjustedSecond, speed, minDistance));
    }


    /// <summary>
    /// 统一处理巡逻点的调整，包含墙壁检测和距离补偿
    /// </summary>
    /// <param name="first">第一个巡逻点</param>
    /// <param name="second">第二个巡逻点</param>
    /// <returns>调整后的两个巡逻点</returns>
    protected virtual (Vector2, Vector2) AdjustPatrolPoints(Vector2 first, Vector2 second)
    {
        const float wallSafeDistance = 9f;
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

    IEnumerator TwoPointPatrolCoroutine(Vector2 first, Vector2 second, float speed)
    {
        while (true)
        {
            yield return MoveToTarget(second, speed);
            yield return MoveToTarget(first, speed);
        }
    }

    IEnumerator RandomRangePatrolCoroutine(Vector2 first, Vector2 second, float speed, float minDistance)
    {
        while (true)
        {
            yield return MoveToTarget(new Vector2(UnityEngine.Random.Range(transform.position.x + minDistance, second.x), second.y), speed);
            yield return MoveToTarget(new Vector2(UnityEngine.Random.Range(first.x, transform.position.x - minDistance), first.y), speed);
        }
    }

    public Action finishMoved;
    IEnumerator MoveToTarget(Vector2 target, float speed)
    {
        float tolerance = 1f;
        while (Mathf.Abs(transform.position.x - target.x) > tolerance)
        {
            float direction = (target.x - transform.position.x) > 0 ? 1 : -1;
            rb.linearVelocityX = direction * speed;
            yield return null;
        }

        rb.linearVelocityX = 0;
        transform.position = new Vector3(target.x, transform.position.y, transform.position.z);
        finishMoved?.Invoke();

        yield return null;
    }

    /// <summary>
    /// 往左往右发射射线检测玩家
    /// </summary>
    /// <param name="range">检测范围</param>
    /// <returns>是否检测到玩家</returns>
    public virtual bool DetectPlayer(float range)
    {
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, Vector2.right, range, 1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.left, range, 1 << LayerMask.NameToLayer("Player"));
        if (hit1.collider != null || hit2.collider != null)
        {
            return true;
        }
        else
            return false;
    }

    public virtual void ChasePlayer(float speed)
    {
        float tolerance = 1f;
        if (Mathf.Abs(transform.position.x - PlayerFSM.Instance.transform.position.x) > tolerance)
        {
            float direction = (PlayerFSM.Instance.transform.position.x - transform.position.x) > 0 ? 1 : -1;
            rb.linearVelocityX = direction * speed;
        }
        else
            rb.linearVelocityX = 0;
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

}