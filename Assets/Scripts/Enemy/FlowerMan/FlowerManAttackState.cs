using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FlowerManAttackState : IState
{
    private FlowerManFSM fSM;

    public FlowerManAttackState(FlowerManFSM fSM)
    {
        this.fSM = fSM;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
        var tc = fSM.GetComponent<TargetCollect>();
        if (tc.attackTarget.Count == 0)
        {
            fSM.ChangeState(FlowerManStateType.Return);
        }
        else
        {
            var target = tc.attackTarget.First();
            Shoot(target);
        }
    }
    public float shootCD => fSM.parameters.shootCD;
    public float _shootTick;
    public void Shoot(GameObject target)
    {
        var tw = fSM.GetOrAddComponent<Tween>();
        var tr = tw.GetOrAddTrack("Shoot");
        if (tr._tweenState == Tween.TweenState.RUNNING)
            return;
        tr.AddTween(_ => ShootImp(target), 0.0, 0.0, 0.0f)
            .AddTween(_ => { }, 0, 0, shootCD).Play();
    }
    public void ShootImp(GameObject target)
    {
        var flowerIns = Object.Instantiate(fSM.parameters.flowerProjectile);
        flowerIns.transform.position = fSM.transform.position;
        var rb = flowerIns.GetComponent<Rigidbody2D>();
        rb.linearVelocity = (target.transform.position - fSM.transform.position).normalized * fSM.parameters.flowerSpeed;
    }

    public void OnUpdate()
    {
    }
}
