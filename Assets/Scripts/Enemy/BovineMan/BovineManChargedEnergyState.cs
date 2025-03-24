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
        fSM.rb.linearVelocity = Vector3.zero;
        chargeTimer = 0.0f;
        float randomValue = Random.value;
        float chargingDuration = (parameters.chargingDurationUpper - parameters.chargingDurationLower) * randomValue + parameters.chargingDurationLower;
        fSM.parameters.chargingDuration = chargingDuration;
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
        if(!fSM.DetectPlayer(parameters.attackDetectRange) && !fSM.IsBubbleInFront(parameters.attackDetectRange))
        {
            detectTimer += Time.deltaTime;
            if(detectTimer >= parameters.canCancelChargingDuration)
                fSM.ChangeState(BovineManStateType.Patrol);
        }
        // 蓄力完成后冲刺攻击
        else if(chargeTimer >= fSM.parameters.chargingDuration)
        {
            fSM.ChangeState(BovineManStateType.SprintAttack);
        }
        // 检测到泡泡或者玩家进入攻击范围
        else if(fSM.DetectPlayer(parameters.attackDetectRange) || fSM.DetectBubble(parameters.attackDetectRange, out var _))
        {
            GameObject bubble;
            if(fSM.DetectBubble(parameters.attackDetectRange, out bubble))
                fSM.parameters.sprintDirection = fSM.transform.position.x > bubble.transform.position.x ? -1 : 1;
            else
                fSM.parameters.sprintDirection = fSM.transform.position.x > PlayerFSM.Instance.transform.position.x ? -1 : 1;
            detectTimer = 0.0f;
        }
    }
}