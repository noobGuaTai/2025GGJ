using UnityEngine;

public class KilledByItemDrop : MonoBehaviour
{
    public LayerMask layer;
    void OnCollisionEnter2D(Collision2D other)
    {
        if (((1 << other.gameObject.layer) & layer) != 0)
        {
            float angle = Vector2.Angle(Vector2.up, (other.transform.position - transform.position).normalized);
            if (angle < 60f)
            {
                if (TryGetComponent<EnemyFSM>(out var e)) e.Die();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & layer) != 0)
        {
            float angle = Vector2.Angle(Vector2.up, (other.transform.position - transform.position).normalized);
            if (angle < 60f)
            {
                if (TryGetComponent<EnemyFSM>(out var e)) e.Die();
            }
        }
    }
}