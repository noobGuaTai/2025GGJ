using UnityEngine;

public class KilledByHeavyObject : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<EnemyFSM>(out var e))
        {
            if (e.somatoType == EnemyFSM.EnemySomatoType.Heavy)
            {
                if (TryGetComponent<EnemyFSM>(out var ee)) ee.Die();
            }
        }
    }
}