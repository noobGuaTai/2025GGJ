using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TreeManGrowState : IState
{
    private TreeManFSM fSM;

    public TreeManGrowState(TreeManFSM fSM)
    {
        this.fSM = fSM;
    }
    public void PlayAnimation()
    {

    }

    public bool growFinish;
    public GameObject growMirage;
    const float growAnimationTime = 0.5f;
    public void OnEnter()
    {
        // fSM.rb.bodyType = RigidbodyType2D.Dynamic;
        growFinish = false;
        PlayAnimation();
        // fSM.animator.speed *= -1;
        fSM.animator.Play("grow_down", 0, 0);

        var tw = fSM.GetOrAddComponent<Tween>();
        tw.AddTween("grawProcess", (x) => { }, 0, 0, 0.5f).
        AddTween(_ => growFinish = true, 0, 0, 0).
        Play();
        fSM.parameters.growAudio.Play();
    }

    public void OnExit()
    {
        // fSM.rb.bodyType = RigidbodyType2D.Kinematic;
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
