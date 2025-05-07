using UnityEngine;

public class Flowers : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerFSM.Instance.Die();
            Destroy(gameObject);
        }

    }
}