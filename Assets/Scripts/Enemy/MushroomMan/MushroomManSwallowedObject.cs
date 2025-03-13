using UnityEngine;

public class MushroomManSwallowedObject : SwallowedObject
{
    MushroomManFSM fsm;
    public override void Start()
    {
        base.Start();
        fsm = GetComponent<MushroomManFSM>();
    }
    public override void OnBreak(BaseBubble bubble)
    {
        base.OnBreak(bubble);
        fsm.ChangeState(MushroomManStateType.Patrol);
    }

    public override void OnLoad(BaseBubble bubble)
    {
        base.OnLoad(bubble);
        fsm.ChangeState(MushroomManStateType.UnderSwallowed);
    }
}
