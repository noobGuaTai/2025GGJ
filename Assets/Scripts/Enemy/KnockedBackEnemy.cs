using System;
using UnityEngine;

public class KnockedBackEnemy : KnockedBackObject
{
    public Action onKnockedBackActions;
    public override void Start()
    {
        base.Start();
    }
    public override void OnKnockedBack()
    {
        base.OnKnockedBack();
        onKnockedBackActions?.Invoke();
    }
}