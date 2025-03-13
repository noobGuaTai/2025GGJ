using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeManAttackState : IState
{
    private TreeManFSM fSM;

    public TreeManAttackState(TreeManFSM fSM)
    {
        this.fSM = fSM;
    }
    public GameObject Shoot(Vector3 position)
    {
        return null;
    }
    GameObject sapling;
    public void OnEnter()
    {
        Shoot(fSM.GetComponent<TargetCollect>().attackTarget.First().transform.position);
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
        if (sapling.GetComponent<Sapling>().impacted)
        {
            fSM.parameters.growPosition = sapling.transform.position;
            Object.Destroy(sapling);
            fSM.ChangeState(TreeManStateType.Grow);
        }
    }

    public void OnUpdate()
    {
    }
}
