using System.Collections;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public Rigidbody2D rb;
    Animator animator;
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    public virtual void ResetSelf()
    {

    }

    public virtual Coroutine TwoPointPatrol(Vector2 first, Vector2 second, float speed)
    {
        return StartCoroutine(TwoPointPatrolCoroutine(first, second, speed));
    }

    IEnumerator TwoPointPatrolCoroutine(Vector2 first, Vector2 second, float speed)
    {
        while (true)
        {
            yield return StartCoroutine(MoveToTarget(second, speed));
            yield return StartCoroutine(MoveToTarget(first, speed));
        }
    }

    IEnumerator MoveToTarget(Vector2 target, float speed)
    {
        float tolerance = 0.1f;
        while (Mathf.Abs(transform.position.x - target.x) > tolerance)
        {
            float direction = (target.x - transform.position.x) > 0 ? 1 : -1;
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocityY);
            yield return null;
        }

        rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
        transform.position = new Vector3(target.x, transform.position.y, transform.position.z);

        yield return null;
    }

    // public virtual void DetectPlayer()
    // {
    //     // 从怪物位置发射一条向前的 Ray，检测玩家
    //     RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, detectionRange, playerLayer);

    //     // 如果 Raycast 检测到物体并且物体属于玩家 Layer
    //     if (hit.collider != null)
    //     {
    //         if (hit.collider.CompareTag("Player"))
    //         {
    //             Debug.Log("玩家在检测范围内！");
    //             // 在这里可以加入其他逻辑，例如攻击玩家或跟随玩家等
    //         }
    //     }
    //     else
    //     {
    //         Debug.Log("没有玩家在检测范围内");
    //     }
    // }
}