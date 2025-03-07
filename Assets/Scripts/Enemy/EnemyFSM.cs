using System.Collections;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public Vector2 initPos;
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initPos = transform.position;
    }
    public virtual void ResetSelf()
    {

    }

    public virtual void ReturnToInitPos()
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
            rb.linearVelocityX = direction * speed;
            yield return null;
        }

        rb.linearVelocityX = 0;
        transform.position = new Vector3(target.x, transform.position.y, transform.position.z);

        yield return null;
    }

    public virtual bool DetectPlayer(float range)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, LayerMask.NameToLayer("Player"));

        if (hit.collider != null)
        {
            return true;
        }
        else
            return false;
    }

    public virtual void ChasePlayer(float speed)
    {
        float tolerance = 0.1f;
        if (Mathf.Abs(transform.position.x - PlayerFSM.Instance.transform.position.x) > tolerance)
        {
            float direction = (PlayerFSM.Instance.transform.position.x - transform.position.x) > 0 ? 1 : -1;
            rb.linearVelocityX = direction * speed;
        }
        else
            rb.linearVelocityX = 0;
    }
}