using UnityEngine;

public class SwallowedObject : MonoBehaviour
{
    public virtual void OnLoad(Bubble bubble)
    {

    }
    public virtual void OnBreak(Bubble bubble)
    {
        transform.SetParent(bubble.transform, false);
    }
}