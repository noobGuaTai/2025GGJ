using NUnit.Framework;
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
    }

    override public void OnFixedUpdate()
    {
    }

    override public void OnUpdate()
    {
        // 计算当前速度
        barkingTimer += Time.deltaTime;
        parameters.currentSpeed = startSpeed - barkingTimer * parameters.retardedVelocity;
        // 刹车
        fsm.Braking();
        if(parameters.currentSpeed <= 0f || Mathf.Abs(fsm.rb.linearVelocityX) < 1.0f)
            fsm.ChangeState(BovineManStateType.Patrol);
    }
}