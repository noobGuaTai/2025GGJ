using UnityEngine;

public class TreeManSapling : MonoBehaviour
{
    public bool impacted;
    void OnCollisionEnter2D(Collision2D other)
    {
        impacted = true;    
    }
}