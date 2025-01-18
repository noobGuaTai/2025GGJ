using Unity.Mathematics;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Collider2D detect;
    public float moveSpeed = 100f;
    private Rigidbody2D rb;
    public GameObject player;
    public Vector2 initPos;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        initPos = transform.position;
    }



    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            rb.linearVelocityX = Mathf.Sign(player.transform.position.x - transform.position.x) * moveSpeed;
            transform.localScale = new Vector3(Mathf.Sign(transform.position.x - player.transform.position.x), 1, 1);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            rb.linearVelocityX = 0;
        }
    }

    public void ResetSelf()
    {
        transform.position = initPos;
    }
}
