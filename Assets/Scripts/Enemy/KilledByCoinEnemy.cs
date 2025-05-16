using UnityEngine;

public class KilledByCoinEnemy : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("WeaponCoin"))
        {
            if (other.gameObject.GetComponent<Rigidbody2D>().linearVelocity.magnitude > 60f)
            {
                if (TryGetComponent<EnemyFSM>(out var e)) e.Die();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("WeaponCoin"))
        {
            if (other.gameObject.GetComponent<Rigidbody2D>().linearVelocity.magnitude > 60f)
            {
                if (TryGetComponent<EnemyFSM>(out var e)) e.Die();
            }
        }
    }
}
