using UnityEngine;
using UnityEngine.InputSystem;

public class Blow : MonoBehaviour
{
    public GameObject player;
    public float blowForce = 1000f;
    public Vector2 direction;

    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        BlowDirectly(other);
    }

    void BlowByMouseDirection(Collider2D other)
    {
        Bubble bubbleScript = other.GetComponent<Bubble>();
        if (bubbleScript != null && bubbleScript.rb != null && player != null)
        {
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            mouseWorldPosition.z = 0f;
            Vector3 playerPosition = player.transform.position;
            Vector2 forceDirection = ((Vector2)(mouseWorldPosition - playerPosition)).normalized;
            bubbleScript.rb.AddForce(forceDirection * blowForce, ForceMode2D.Impulse);
        }
        gameObject.SetActive(false);
    }

    void BlowDirectly(Collider2D other)
    {
        Bubble bubbleScript = other.GetComponent<Bubble>();
        if (bubbleScript != null && bubbleScript.rb != null && player != null)
        {
            bubbleScript.rb.AddForce(direction * blowForce, ForceMode2D.Impulse);
        }
        gameObject.SetActive(false);
    }
}

