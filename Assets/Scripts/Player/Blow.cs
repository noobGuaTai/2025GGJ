using UnityEngine;
using UnityEngine.InputSystem;

public class Blow : MonoBehaviour
{
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
        if (other.TryGetComponent<BaseBubble>(out var bubbleScript))
        {
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            mouseWorldPosition.z = 0f;
            Vector3 playerPosition = PlayerFSM.Instance.transform.position;
            Vector2 forceDirection = ((Vector2)(mouseWorldPosition - playerPosition)).normalized;
            bubbleScript.rb.AddForce(forceDirection * blowForce, ForceMode2D.Impulse);
        }
        gameObject.SetActive(false);
    }

    void BlowDirectly(Collider2D other)
    {
        if (other.TryGetComponent<BaseBubble>(out var bubbleScript))
        {
            bubbleScript.rb.AddForce(direction * blowForce, ForceMode2D.Impulse);
        }
        gameObject.SetActive(false);
    }
}

