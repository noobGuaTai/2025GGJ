using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeManAttackState : IState
{
    private TreeManFSM fSM;
    public GameObject saplingIns;

    public TreeManAttackState(TreeManFSM fSM)
    {
        this.fSM = fSM;
    }
    public GameObject Shoot(Vector3 targetPos)
    {
        var saplingIns = Object.Instantiate(fSM.parameters.saplingPrefab);
        var rb = saplingIns.GetComponent<Rigidbody2D>();
        rb.linearVelocity = ProjectileMotion.CalculateInitialVelocity(
            fSM.transform.position, targetPos, 45);
        
        return saplingIns;
    }
    public void OnEnter()
    {
        Shoot(fSM.GetComponent<TargetCollect>().attackTarget.First().transform.position);
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
        if (saplingIns.GetComponent<Sapling>().impacted)
        {
            fSM.parameters.growPosition = saplingIns.transform.position;
            Object.Destroy(saplingIns);
            fSM.ChangeState(TreeManStateType.Grow);
        }
    }

    public void OnUpdate()
    {
    }
}
