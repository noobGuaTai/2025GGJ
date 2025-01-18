using Unity.Mathematics;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Collider2D detect;
    public float moveSpeed = 100f;
    private Rigidbody2D rb;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            rb.linearVelocityX = Mathf.Sign(player.transform.position.x - transform.position.x) * moveSpeed;
        }
        else
        {
            rb.linearVelocityX = 0;
        }
    }
}
