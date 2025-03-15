using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FlowerManReturnState : IState
{
    private FlowerManFSM fSM;

    public FlowerManReturnState(FlowerManFSM fSM)
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
        var mo = fSM.GetComponent<SpriteMotion>();
        var sp = mo.startPosition;

        var offset = sp - fSM.transform.position;
        if (offset.Length() < 5)
        {
            fSM.ChangeState(FlowerManStateType.Patrol);
            return;
        }
        fSM.rb.linearVelocity = offset.normalized * fSM.parameters.returnSpeed;
    }

    public void OnUpdate()
    {
    }
}
