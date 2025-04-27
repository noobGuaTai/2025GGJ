using UnityEngine;

public class BovineManPatrolState : BovineBaseState
{
    Coroutine patrolCoroutine;
    public BovineManPatrolState(BovineManFSM _fsm) : base(_fsm)
    {
    }

    override public void OnEnter()
    {
        if(param.patrolPoint.Length == 0)
        {
            param.patrolPoint = new float[2] { -50f, 50f };
        }
    }

    override public void OnExit()
    {
        if(patrolCoroutine != null)
            fsm.StopCoroutine(patrolCoroutine);
        patrolCoroutine = null;
    }

    override public void OnFixedUpdate()
    {
    }

    override public void OnUpdate()
    {
        // Debug.Log("is on ground: " + fsm.param.isOnGround);
        StartPatrol();
        if(fsm.IsDetectObjectByLayer(fsm.param.detectRange, LayerMask.GetMask("Player", "Bubble"), out var _))
            fsm.ChangeState(BovineManStateType.ChargedEnergy);
    }

    void StartPatrol()
    {
        if(patrolCoroutine == null && fsm.param.isOnGround)
        {
            fsm.initPos = fsm.transform.position;
            patrolCoroutine = fsm.TwoPointPatrol(
                new Vector2(fsm.initPos.x + param.patrolPoint[0], fsm.initPos.y),
                new Vector2(fsm.initPos.x + param.patrolPoint[1], fsm.initPos.y),
                param.patrolSpeed
            );
            Debug.Log("patrol set" + param.patrolSpeed + param.patrolPoint);
        }
    }
}