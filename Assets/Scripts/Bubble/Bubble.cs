using UnityEngine;

public class Bubble : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        Invoke("Escape", 1f);
    }


    void Update()
    {

    }

    void Escape()
    {
        transform.SetParent(transform.Find("/Root/Bubbles"));
    }

    void Swallow(GameObject g)
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<SwallowedObject>() != null)
        {
            Swallow(other.gameObject);
        }
    }
}
