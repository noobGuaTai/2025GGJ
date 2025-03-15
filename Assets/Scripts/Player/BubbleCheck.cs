using System.Linq;
using UnityEngine;

public class BubbleCheck : AnythingCheck
{
    public override bool Check()
    {
        return anythingCheckpoints.Any(check => Physics2D.OverlapCircle(check.position, checkRadius, layer)?.GetComponent<BigBubble>() != null);
    }
}
