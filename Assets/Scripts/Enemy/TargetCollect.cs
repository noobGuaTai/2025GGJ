using System.Collections.Generic;
using UnityEngine;

public class TargetCollect : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (TargetLayer(other.gameObject))
            attackTarget.Add(other.gameObject);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (TargetLayer(other.gameObject))
            attackTarget.Remove(other.gameObject);
    }
    public bool TargetLayer(GameObject gameObject)
        => gameObject.layer == LayerMask.NameToLayer("Player") || gameObject.layer == LayerMask.NameToLayer("Bubble");
    public HashSet<GameObject> attackTarget = new();
}