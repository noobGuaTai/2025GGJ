using UnityEngine;

public class KnockedBackEnemy : KnockedBackObject
{
    EnemyFSM fsm;
    public override void Start()
    {
        base.Start();
        fsm = GetComponent<EnemyFSM>();
    }
    public override void OnKnockedBack()
    {
        base.OnKnockedBack();
        fsm.OnKnockedBack();
    }
}