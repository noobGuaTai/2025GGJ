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
    }

    override public void OnFixedUpdate()
    {
    }

    override public void OnUpdate()
    {
        // 计算当前速度
        if(parameters.isOnGround)
            sprintTimer += Time.deltaTime;
        parameters.currentSpeed = getSpeed();
        fSM.ChasePlayer();
        // 玩家不在攻击范围内，一段时间后开始刹车
        if(!fSM.IsPlayerInFront(parameters.attackDetectRange))
        {
            detectTimer += Time.deltaTime;
            if(detectTimer >= parameters.canCancelSprintDuration)
                fSM.ChangeState(BovineManStateType.Braking);
        }
        // 玩家在攻击范围内
        if(fSM.IsPlayerInFront(parameters.attackDetectRange))
            detectTimer = 0.0f;
        // # FIXME: 撞击玩家需要其他API
        // 撞击到玩家开始刹车
        float tolerance = 1f;
        if(Vector2.Distance(PlayerFSM.Instance.transform.position, fSM.transform.position) <= tolerance || Mathf.Abs(fSM.rb.linearVelocityX) < 1.0f)
            fSM.ChangeState(BovineManStateType.Braking);
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
        return Mathf.Min(speed, parameters.maxVelocity);
    }
}