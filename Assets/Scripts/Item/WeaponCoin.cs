using System;
using UnityEngine;

public class WeaponCoin : MonoBehaviour
{
    public Action onInit;
    public AudioSource collectAudio;
    public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        onInit?.Invoke();
        Destroy(gameObject, 8f);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<KilledByCoinEnemy>(out var k))
        {
            float angle = Vector2.Angle(Vector2.up, (transform.position - other.transform.position).normalized);
            if (angle <= 60f)
            {
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}