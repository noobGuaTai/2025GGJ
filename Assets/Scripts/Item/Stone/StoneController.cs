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
        // if (other.gameObject.TryGetComponent<EnemyFSM>(out var e))
        // {
        //     if (e.somatoType == EnemyFSM.EnemySomatoType.Light)
        //     {
        //         float angle = Vector2.Angle(Vector2.up, (other.transform.position - transform.position).normalized);
        //         if (angle < 60f)
        //         {
        //             other.gameObject.SetActive(false);
        //         }
        //     }

        // }
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
