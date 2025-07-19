using UnityEngine;

public class KilledByEnemyDrop : MonoBehaviour
{
    public EnemyFSM.EnemySomatoType somatoType;
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<EnemyFSM>(out var e))
        {
            if (e.somatoType == somatoType)
            {
                var eSpeed = e.GetComponent<Rigidbody2D>().linearVelocityY;
                var thisSpeed = GetComponent<Rigidbody2D>().linearVelocityY;
                float angle = Vector2.Angle(Vector2.up, (other.transform.position - transform.position).normalized);
                if (angle < 45f && Mathf.Abs(eSpeed - thisSpeed) > 10f)
                {
                    GetComponent<EnemyFSM>().Die();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<EnemyFSM>(out var e))
        {
            if (e.somatoType == somatoType)
            {
                var eSpeed = e.GetComponent<Rigidbody2D>().linearVelocityY;
                var thisSpeed = GetComponent<Rigidbody2D>().linearVelocityY;
                float angle = Vector2.Angle(Vector2.up, (other.transform.position - transform.position).normalized);
                if (angle < 45f && Mathf.Abs(eSpeed - thisSpeed) > 10f)
                {
                    GetComponent<EnemyFSM>().Die();
                }
            }
        }
    }
}
