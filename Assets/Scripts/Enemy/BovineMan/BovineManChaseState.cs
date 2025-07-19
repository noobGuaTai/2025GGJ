using UnityEngine;

public class BovineManChaseState : BovineBaseState
{
    public BovineManChaseState(BovineManFSM _fsm) : base(_fsm)
    {
    }

    override public void OnEnter()
    {
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
        param.chaseSpeed = getSpeed();
        if(fsm.IsDetectObjectByLayer(fsm.param.detectRange, LayerMask.GetMask("Bubble"), out var b))
        {
            fsm.InertialChaseObject(fsm.param.chaseSpeed, b);
            return;
        }
        if(fsm.IsDetectObjectByLayer(fsm.param.detectRange, LayerMask.GetMask("Player"), out var p))
        {
            fsm.InertialChaseObject(fsm.param.chaseSpeed, p);
            return;
        }
        fsm.rb.AddForce(new Vector2(-fsm.param.decelerateSpeed * Mathf.Sign(fsm.rb.linearVelocityX), 0), ForceMode2D.Impulse);
        if (Mathf.Abs(fsm.rb.linearVelocityX) < 0.1f)
            fsm.ChangeState(BovineManStateType.Return);
    }

    // 基础速度 + 蓄力速度 + 
    private float getSpeed()
    {
        // 基础速度
        float baseSpeed = param.sprintBaseSpeed;
        // 蓄力速度
        float chargingSpeed = param.speedIncrementOnChargingPerSec * param.chargingDuration;
        return baseSpeed + chargingSpeed;
    }
}