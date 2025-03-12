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
    public enum EnemySomatotype
    {
        Light,
        Heavy
    }
    public EnemySomatotype somatotype = EnemySomatotype.Light;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initPos = transform.position;
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
        return StartCoroutine(TwoPointPatrolCoroutine(first, second, speed));
    }

    IEnumerator TwoPointPatrolCoroutine(Vector2 first, Vector2 second, float speed)
    {
        while (true)
        {
            yield return MoveToTarget(second, speed);
            yield return MoveToTarget(first, speed);
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

}