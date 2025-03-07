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
        float chargingDuration = (parameters.chargingDurationUpper - parameters.chargingDurationLower) * randomValue + parameters.chargingDurationLower;
        fsm.parameters.chargingDuration = chargingDuration;
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
        if(!fsm.DetectPlayer(parameters.attackDetectRange))
        {
            detectTimer += Time.deltaTime;
            if(detectTimer >= parameters.canCancelChargingDuration)
                fsm.ChangeState(BovineManStateType.Patrol);
        }
        // 蓄力完成后冲刺攻击
        else if(chargeTimer >= fsm.parameters.chargingDuration)
        {
            fsm.parameters.sprintDirection = fsm.transform.position.x > PlayerFSM.Instance.transform.position.x ? -1 : 1;
            fsm.ChangeState(BovineManStateType.SprintAttack);
        }
        // 检测到玩家进入攻击范围
        else if(fsm.DetectPlayer(parameters.attackDetectRange))
        {
            detectTimer = 0.0f;
        }
    }
}