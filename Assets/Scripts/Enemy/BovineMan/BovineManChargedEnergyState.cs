using UnityEngine;

public class BovineManChargedEnergyState : BovineBaseState
{
    private float chargeTimer;
    private float detectTimer;
    public BovineManChargedEnergyState(BovineManFSM _fsm) : base(_fsm)
    {
    }

    override public void OnEnter()
    {
        fsm.rb.linearVelocity = Vector3.zero;
        chargeTimer = 0.0f;
        float randomValue = Random.value;
        float chargingDuration = (param.chargingDurationUpper - param.chargingDurationLower) * randomValue + param.chargingDurationLower;
        fsm.param.chargingDuration = chargingDuration;
        fsm.attackAudio.Play();
    }

    override public void OnExit()
    {
    }

    override public void OnFixedUpdate()
    {
    }

    override public void OnUpdate()
    {
        chargeTimer += Time.deltaTime;
        // 不在攻击检测范围内一段时候后取消蓄力
        if (!fsm.IsDetectObjectByLayer(fsm.param.detectRange, LayerMask.GetMask("Player", "Bubble"), out var _))
        {
            detectTimer += Time.deltaTime;
            if (detectTimer >= param.canCancelChargingDuration)
                fsm.ChangeState(BovineManStateType.Patrol);
        }
        // 蓄力完成后冲刺攻击
        else if (chargeTimer >= fsm.param.chargingDuration)
        {
            fsm.ChangeState(BovineManStateType.Chase);
        }
        // 检测到泡泡或者玩家进入攻击范围
        else if (fsm.IsDetectObjectByLayer(fsm.param.detectRange, LayerMask.GetMask("Player", "Bubble"), out var _))
        {
            detectTimer = 0.0f;
        }
    }
}