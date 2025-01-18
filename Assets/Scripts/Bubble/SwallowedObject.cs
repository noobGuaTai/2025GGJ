using UnityEngine;

public class SwallowedObject : MonoBehaviour
{
    public virtual void OnLoad(Bubble bubble)
    {
        transform.SetParent(bubble.transform, false);
    }
    public virtual void OnBreak(Bubble bubble)
    {

    }
}