using UnityEngine;

public class BovineManBrakingState : BovineBaseState
{
    private float startSpeed;
    private float barkingTimer;
    public BovineManBrakingState(BovineManFSM _fsm) : base(_fsm)
    {
    }

    override public void OnEnter()
    {
        startSpeed = parameters.currentSpeed;
        barkingTimer = 0f;
    }

    override public void OnExit()
    {
        fsm.rb.linearVelocity = Vector3.zero;
    }

    override public void OnFixedUpdate()
    {
    }

    override public void OnUpdate()
    {
        barkingTimer += Time.deltaTime;
        parameters.currentSpeed = startSpeed - barkingTimer * parameters.retardedVelocity;
        fsm.ChasePlayer();
        if(parameters.currentSpeed <= 0f)
        {
            fsm.ChangeState(BovineManStateType.Patrol);
        }
    }
}