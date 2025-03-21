using System;
using UnityEngine;

public class SwallowedEnemy : SwallowedObject
{
    public Action onBreakActions;
    public Action onLoadActions;
    public override void Start()
    {
        base.Start();
    }
    public override void OnBreak(BaseBubble bubble)
    {
        base.OnBreak(bubble);
        onBreakActions?.Invoke();
    }

    public override void OnLoad(BaseBubble bubble)
    {
        base.OnLoad(bubble);
        onLoadActions?.Invoke();
    }
}