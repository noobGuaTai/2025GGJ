using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FlowerManAttackState : IState
{
    private FlowerManFSM fSM;

    float shootTimer => fSM.parameters.shootTimer;
    GameObject flowerProjectileInstance;
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
        if (fSM.IsDetectObjectByLayer(fSM.parameters.attackRange, LayerMask.GetMask("Player"), out var target))
        {
            Shoot(target);
        }
        else
        {
            fSM.ChangeState(FlowerManStateType.Patrol);
        }
    }
    // public float shootCD => fSM.parameters.shootCD;
    // public float _shootTick;
    // public void Shoot(GameObject target)
    // {
    //     var tw = fSM.GetOrAddComponent<Tween>();
    //     var tr = tw.GetOrAddTrack("Shoot");
    //     if (tr._tweenState == Tween.TweenState.RUNNING)
    //         return;
    //     tr.AddTween(_ => ShootImp(target), 0.0, 0.0, 0.0f)
    //         .AddTween(_ => { }, 0, 0, shootCD).Play();
    // }
    // public void ShootImp(GameObject target)
    // {
    //     var flowerIns = Object.Instantiate(fSM.parameters.flowerProjectile);
    //     flowerIns.transform.position = fSM.transform.position;
    //     var rb = flowerIns.GetComponent<Rigidbody2D>();
    //     rb.linearVelocity = (target.transform.position - fSM.transform.position).normalized * fSM.parameters.flowerSpeed;
    // }

    void Shoot(GameObject target)
    {
        if (shootTimer < fSM.parameters.shootCD)
            return;
        // fSM.animator.Play("attack", 0, 0);
        fSM.parameters.shootTimer = 0;
        // flowerProjectileInstance = GameObject.Instantiate(fSM.parameters.flowerProjectilePrefab, fSM.transform.position, Quaternion.identity);
        // flowerProjectileInstance.transform.parent = fSM.transform;
        fSM.Attack(target);

        // flowerProjectileInstance.transform.SetParent(fSM.transform, false);
        // flowerProjectileInstance.transform.localPosition = Vector2.zero;

        // var f = fSM.parameters.flowerProjectilePrefab.GetComponent<FlowerManProjectile>();
        // f.speed = fSM.parameters.flowerSpeed;
        // f.aim = target;
        // f.father = fSM.gameObject;
    }

    public void OnUpdate()
    {
        if (flowerProjectileInstance != null)
            flowerProjectileInstance.transform.position = fSM.transform.position;
    }
}
