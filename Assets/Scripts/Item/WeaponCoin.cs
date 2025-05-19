using System;
using UnityEngine;

public class WeaponCoin : MonoBehaviour
{
    public Action onInit;
    public AudioSource collectAudio;
    public Rigidbody2D rb;
    float timer;
    public Collider2D colliderPlayer;
    float initGravityScale;

    AnythingCheck anythingCheck => GetComponent<AnythingCheck>();
    Animator animator => GetComponent<Animator>();
    AnimatorStateInfo currentStateInfo => animator.GetCurrentAnimatorStateInfo(0);
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        onInit?.Invoke();
        colliderPlayer.enabled = false;
        initGravityScale = rb.gravityScale;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // if (other.gameObject.TryGetComponent<KilledByCoinEnemy>(out var k))
        // {
        //     // float angle = Vector2.Angle(Vector2.up, (transform.position - other.transform.position).normalized);
        //     // if (angle <= 60f)
        //     // {
        //     //     Destroy(other.gameObject);
        //     //     Destroy(gameObject);
        //     // }
        //     if (rb.linearVelocity.magnitude > 60)
        //     {
        //         Destroy(other.gameObject);
        //         Destroy(gameObject);
        //     }
        // }
        rb.gravityScale = initGravityScale;
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerFSM.Instance.param.playerInventory.coins++;
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (anythingCheck.isChecked && currentStateInfo.IsName("drop"))
        {
            animator.Play("idle", 0, 0);
        }
        else if (!anythingCheck.isChecked && currentStateInfo.IsName("idle"))
        {
            animator.Play("drop", 0, 0);
        }

        if (timer > 1)
            return;
        timer += Time.deltaTime;
        if (timer > 1) colliderPlayer.enabled = true;
    }
    public void DestorySelf()
    {
        animator.Play("shoot", 0, 0);
        Destroy(gameObject, 0.625f);
    }
}