using UnityEngine;

public class KilledByPlayerAttack : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            if (TryGetComponent<EnemyFSM>(out var e)) e.Die();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            if (TryGetComponent<EnemyFSM>(out var e)) e.Die();
        }
    }
}