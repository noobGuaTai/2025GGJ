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
            // parameters.patrolPoint = new Vector2[2] { fSM.transform.position - Vector3.right * 50, fSM.transform.position + Vector3.right * 50 };
            param.patrolPoint = new float[2] { -50f, 50f };
        }
        // patrolCoroutine = fSM.TwoPointPatrol(parameters.patrolPoint[0], parameters.patrolPoint[1], p arameters.patrolSpeed);
        // parameters.currentSpeed = parameters.patrolSpeed;
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
        }
    }
}