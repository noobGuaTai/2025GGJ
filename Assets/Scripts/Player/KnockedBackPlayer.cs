using UnityEngine;

public class KnockedBackPlayer : KnockedBackObject
{
    public override void Start()
    {
        base.Start();
    }
    public override void OnKnockedBack()
    {
        base.OnKnockedBack();
        PlayerFSM.Instance.OnKnockedBack();
    }
}