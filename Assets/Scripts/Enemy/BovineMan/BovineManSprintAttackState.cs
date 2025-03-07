using UnityEngine;

public class BovineManSprintAttackState : BovineBaseState
{
    private float sprintTimer = 0f;
    private float detectTimer = 0f;
    public BovineManSprintAttackState(BovineManFSM _fsm) : base(_fsm)
    {
    }

    override public void OnEnter()
    {
        sprintTimer = 0f;
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
        sprintTimer += Time.deltaTime;
        parameters.currentSpeed = getSpeed();
        fsm.ChasePlayer();
        // 玩家不在攻击范围内，一段时间后开始刹车
        if(!fsm.DetectPlayer(parameters.attackDetectRange))
        {
            detectTimer += Time.deltaTime;
            if(detectTimer >= parameters.canCancelSprintDuration)
            {
                fsm.ChangeState(BovineManStateType.Braking);
            }
        }

    }

    // 基础速度 + 蓄力速度 + 
    private float getSpeed()
    {
        // 基础速度
        float baseSpeed = parameters.sprintBaseSpeed;
        // 蓄力速度
        float chargingSpeed = parameters.speedIncrementOnChargingPerSec * parameters.chargingDuration;
        // 加速速度
        float accelerationSpeed = parameters.acceleratedVelocity * sprintTimer;

        float speed = baseSpeed + chargingSpeed + accelerationSpeed;
        return Mathf.Max(speed, parameters.maxVelocity);
    }
}