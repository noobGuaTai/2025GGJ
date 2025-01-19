using Unity.Mathematics;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Collider2D detect;
    public float moveSpeed = 100f;
    private Rigidbody2D rb;
    public GameObject player;
    public Vector2 initPos;
    private float shootTimer = 0f;
    public float cooldown = 3f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        initPos = transform.position;
    }



    void OnTriggerStay2D(Collider2D other)
    {

    }

    void OnTriggerExit2D(Collider2D other)
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {

        }
    }

    public void ResetSelf()
    {
        transform.position = initPos;
    }
}
