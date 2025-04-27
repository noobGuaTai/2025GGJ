using UnityEngine;

public class FlowerManProjectile : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
