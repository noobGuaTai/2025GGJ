using UnityEngine;

public class BovineManPatrolState : BovineBaseState
{
    // # FIXME: Some API Changed
    Coroutine patrolCoroutine;
    public BovineManPatrolState(BovineManFSM _fsm) : base(_fsm)
    {
    }

    override public void OnEnter()
    {
        if(parameters.patrolPoint.Length == 0)
        {
            // parameters.patrolPoint = new Vector2[2] { fSM.transform.position - Vector3.right * 50, fSM.transform.position + Vector3.right * 50 };
            parameters.patrolPoint = new float[2] { -50f, 50f };
        }
        // patrolCoroutine = fSM.TwoPointPatrol(parameters.patrolPoint[0], parameters.patrolPoint[1], parameters.patrolSpeed);
        // parameters.currentSpeed = parameters.patrolSpeed;
    }

    override public void OnExit()
    {
        if(patrolCoroutine != null)
            fSM.StopCoroutine(patrolCoroutine);
        patrolCoroutine = null;
    }

    override public void OnFixedUpdate()
    {
    }

    override public void OnUpdate()
    {
        StartPatrol();
        if(fSM.DetectPlayer(parameters.attackDetectRange))
            fSM.ChangeState(BovineManStateType.ChargedEnergy);
    }

    void StartPatrol()
    {
        if(patrolCoroutine == null && fSM.parameters.isOnGround)
        {
            fSM.initPos = fSM.transform.position;
            patrolCoroutine = fSM.TwoPointPatrol(
                new Vector2(fSM.initPos.x + parameters.patrolPoint[0], fSM.initPos.y),
                new Vector2(fSM.initPos.x + parameters.patrolPoint[1], fSM.initPos.y),
                parameters.patrolSpeed
            );
            parameters.currentSpeed = parameters.patrolSpeed;
        }
    }
}