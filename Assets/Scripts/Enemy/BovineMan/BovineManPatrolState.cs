using UnityEngine;

public class BovineManPatrolState : BovineBaseState
{
    Coroutine patrolCoroutine;
    public BovineManPatrolState(BovineManFSM _fsm) : base(_fsm)
    {
    }

    override public void OnEnter()
    {
        if(parameters.patrolPoint.Length == 0)
        {
            parameters.patrolPoint = new Vector2[2] { fsm.transform.position - Vector3.right * 50, fsm.transform.position + Vector3.right * 50 };
        }
        patrolCoroutine = fsm.TwoPointPatrol(parameters.patrolPoint[0], parameters.patrolPoint[1], parameters.patrolSpeed);
        parameters.currentSpeed = parameters.patrolSpeed;
    }

    override public void OnExit()
    {
        fsm.StopCoroutine(patrolCoroutine);
    }

    override public void OnFixedUpdate()
    {
    }

    override public void OnUpdate()
    {
        Debug.Log("fsm.DetectPlayer(parameters.attackDetectRange): "+ fsm.DetectPlayer(parameters.attackDetectRange));
        if(fsm.DetectPlayer(parameters.attackDetectRange))
            fsm.ChangeState(BovineManStateType.ChargedEnergy);
    }
}