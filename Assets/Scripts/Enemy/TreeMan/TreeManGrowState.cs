using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TreeManGrowState : IState
{
    private TreeManFSM fSM;

    public TreeManGrowState(TreeManFSM fSM)
    {
        this.fSM = fSM;
    }
    public void PlayAnimation() {
        fSM.animator.SetTrigger("GrowBegin");

    }
    public GameObject CreateGrowMirage() {
        var mirageIns = Object.Instantiate(fSM.parameters.saplingGrowUpMirage);
        return mirageIns;
    }
    public bool growFinish;
    public GameObject growMirage;

    public void OnEnter()
    {
        growFinish = false;
        PlayAnimation();
        growMirage = CreateGrowMirage();
    }

    public void OnExit()
    {
        fSM.animator.SetTrigger("GrowEnd");
    }

    public void OnFixedUpdate()
    {
        if (growFinish)
        {
            fSM.transform.position = fSM.parameters.growPosition;
            Object.Destroy(growMirage);
            fSM.ChangeState(TreeManStateType.Idle);
        }
    }

    public void OnUpdate()
    {
    }
}
