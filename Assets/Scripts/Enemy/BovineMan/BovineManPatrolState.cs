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
            parameters.patrolPoint = new Vector2[2] { fSM.transform.position - Vector3.right * 50, fSM.transform.position + Vector3.right * 50 };
        }
        patrolCoroutine = fSM.TwoPointPatrol(parameters.patrolPoint[0], parameters.patrolPoint[1], parameters.patrolSpeed);
        parameters.currentSpeed = parameters.patrolSpeed;
    }

    override public void OnExit()
    {
        fSM.StopCoroutine(patrolCoroutine);
    }

    override public void OnFixedUpdate()
    {
    }

    override public void OnUpdate()
    {
        if(fSM.DetectPlayer(parameters.attackDetectRange))
            fSM.ChangeState(BovineManStateType.ChargedEnergy);
    }
}