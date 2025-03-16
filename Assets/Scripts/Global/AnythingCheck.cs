using System.Linq;
using UnityEngine;

public class AnythingCheck : MonoBehaviour
{
    public enum CheckMethod
    {
        Any,
        All
    }
    public CheckMethod checkMethod = CheckMethod.Any;
    public Transform[] anythingCheckpoints;
    public float checkRadius = 0.2f;
    public LayerMask layer;
    public bool isChecked;

    void Start()
    {

    }

    public virtual void Update()
    {
        isChecked = Check();
    }

    public virtual bool Check()
    {
        switch (checkMethod)
        {
            case CheckMethod.Any:
                return anythingCheckpoints.Any(check => Physics2D.OverlapCircle(check.position, checkRadius, layer));
            case CheckMethod.All:
                return anythingCheckpoints.All(check => Physics2D.OverlapCircle(check.position, checkRadius, layer));
            default:
                return false;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (anythingCheckpoints != null)
        {
            Gizmos.color = Color.yellow;
            foreach (var check in anythingCheckpoints)
            {
                if (check != null)
                {
                    Gizmos.DrawWireSphere(check.position, checkRadius);
                }
            }
        }
    }
}
