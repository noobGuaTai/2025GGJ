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

    }
    public GameObject CreateGrowMirage() {
        return null;
    }
    public bool animationFinish;
    public GameObject growMirage;

    public void OnEnter()
    {
        animationFinish = false;
        PlayAnimation();
        growMirage = CreateGrowMirage();
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
        if (animationFinish)
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
