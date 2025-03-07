using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomManAttackState : IState
{
    private MushroomManFSM fSM;
    private MushroomManParameters parameters;
    Coroutine returnToInitPosCoroutine;

    public MushroomManAttackState(MushroomManFSM fSM)
    {
        this.fSM = fSM;
        this.parameters = fSM.parameters;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {
        fSM.finishMoved = null;
        // fSM.StopCoroutine(returnToInitPosCoroutine);
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
        if (fSM.DetectPlayer(parameters.detectRange))
        {
            // if (returnToInitPosCoroutine != null)
            //     fSM.StopCoroutine(returnToInitPosCoroutine);
            fSM.ChasePlayer(parameters.chaseSpeed);
        }
        else
        {
            // fSM.finishMoved += () => { fSM.ChangeState(MushroomManStateType.Patrol); };
            // returnToInitPosCoroutine = fSM.ReturnToInitPos(parameters.patrolSpeed);
            fSM.ChangeState(MushroomManStateType.Patrol);
        }

    }


}
