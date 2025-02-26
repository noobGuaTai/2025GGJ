using UnityEngine;

public class StoneController : MonoBehaviour
{
    public Vector2 initPos;
    private Rigidbody2D rb;

    void Start()
    {
        initPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.gameObject.SetActive(false);
        }
    }

    public void ResetSelf()
    {
        transform.position = initPos;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

    }
}
