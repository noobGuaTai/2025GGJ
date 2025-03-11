using UnityEngine;
using UnityEngine.UIElements;

public class SpriteMotion : MonoBehaviour
{
    [Header("Flag")]
    public bool autoFlipX;
    [Header("Info")]
    public Vector3 lastPosition;
    public Vector3 offset;
    public Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
    }
    
    void FixedUpdate()
    {
        offset = transform.position - lastPosition;
        lastPosition = transform.position;
        if (autoFlipX && TryGetComponent<SpriteRenderer>(out var sp))
        {
            var sign = Mathf.Sign(offset.x);
            if (sign != 0)
                sp.flipX = sign > 0;
        }
    }
}