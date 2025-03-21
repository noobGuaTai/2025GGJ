using UnityEngine;

public class Sapling : MonoBehaviour
{
    public bool impacted;
    void OnCollisionEnter2D(Collision2D other)
    {
        impacted = true;    
    }
}