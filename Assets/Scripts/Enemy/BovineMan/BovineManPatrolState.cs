using UnityEngine;

public class BovineManPatrolState : BovineBaseState
{
    public BovineManPatrolState(BovineManFSM _fsm) : base(_fsm)
    {
    }

    override public void OnEnter()
    {
        if(parameters.patrolPoint.Length == 0)
        {
            parameters.patrolPoint = new Vector2[2] { fsm.transform.position - Vector3.right * 5, fsm.transform.position + Vector3.right * 5 };
        }
        fsm.TwoPointPatrol(parameters.patrolPoint[0], parameters.patrolPoint[1], parameters.patrolSpeed);
        parameters.currentSpeed = parameters.patrolSpeed;
    }

    override public void OnExit()
    {
        fsm.StopCoroutine("TwoPointPatrol");
        fsm.rb.linearVelocity = Vector3.zero;
    }

    override public void OnFixedUpdate()
    {
    }

    override public void OnUpdate()
    {
        if(fsm.DetectPlayer(parameters.attackDetectRange))
            fsm.ChangeState(BovineManStateType.ChargedEnergy);
    }
}