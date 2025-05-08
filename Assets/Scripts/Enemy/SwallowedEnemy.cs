using System;
using System.Collections;
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
        StartCoroutine(wait());
    }

    public override void OnLoad(BaseBubble bubble)
    {
        base.OnLoad(bubble);
        onLoadActions?.Invoke();
    }
    IEnumerator wait()
    {
        yield return null;
        onBreakActions?.Invoke();
    }
}