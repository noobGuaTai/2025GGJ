using UnityEngine;

public class TreeManSapling : MonoBehaviour
{
    public bool impacted;
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            GetComponent<AudioSource>().Play();
            impacted = true;
        }
    }
}