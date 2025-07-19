using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManIdleState : IState
{
    private TreeManFSM fSM;

    public TreeManIdleState(TreeManFSM fSM)
    {
        this.fSM = fSM;
    }

    public void OnEnter()
    {
        fSM.animator.Play("idle", 0, 0);
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
        var t = fSM.GetComponent<TargetCollect>();
        if (t.attackTarget.Count != 0)
            fSM.ChangeState(TreeManStateType.Attack);
    }

    public void OnUpdate()
    {
    }
}
