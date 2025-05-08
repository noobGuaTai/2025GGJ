using System;
using UnityEngine;

public class WeaponCoin : MonoBehaviour
{
    public Action onInit;
    public AudioSource collectAudio;
    public Rigidbody2D rb;
    float timer;
    public Collider2D colliderPlayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        onInit?.Invoke();
        colliderPlayer.enabled = false;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<KilledByCoinEnemy>(out var k))
        {
            // float angle = Vector2.Angle(Vector2.up, (transform.position - other.transform.position).normalized);
            // if (angle <= 60f)
            // {
            //     Destroy(other.gameObject);
            //     Destroy(gameObject);
            // }
            if (rb.linearVelocity.magnitude > 60)
            {
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerFSM.Instance.param.playerInventory.coins++;
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (timer > 1)
            return;
        timer += Time.deltaTime;
        if (timer > 1) colliderPlayer.enabled = true;
    }
}