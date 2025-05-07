using System;
using System.Linq;
using UnityEngine;

public class TreeManAttackState : IState
{
    private TreeManFSM fSM;

    public TreeManAttackState(TreeManFSM fSM)
    {
        this.fSM = fSM;
    }
    public GameObject Shoot(Vector3 targetPos)
    {
        Debug.Log($"Shoot {Time.realtimeSinceStartup}");
        var saplingIns = UnityEngine.Object.Instantiate(fSM.parameters.saplingPrefab);
        saplingIns.transform.position = fSM.transform.position;
        var rb = saplingIns.GetComponent<Rigidbody2D>();
        for (var angle = 30; angle < 360; angle += 30)
        {
            if (angle % 90 == 0) continue;
            var v = ProjectileMotion.CalculateInitialVelocity(
                fSM.transform.position, targetPos, angle,
                saplingIns.GetComponent<Rigidbody2D>().gravityScale * 9.81f);
            if (v != Vector3.zero)
            {
                Debug.Log($"TreeMan find initial speed : {v} , angle {angle}");
                rb.linearVelocity = v;
                return saplingIns;
            }
        }
        Debug.Log("TreeMan can not find a nice sapling speed");
        return null;
    }
    public void OnEnter()
    {
        Debug.Log($"ENTER Attack {Time.realtimeSinceStartup}");
        fSM.animator.Play("attack", 0, 0);
        fSM.parameters.cancelAttack = false;
    }

    public void OnExit()
    {
        UnityEngine.Object.Destroy(fSM.parameters.saplingIns);
        fSM.parameters.saplingIns = null;
    }

    public void OnFixedUpdate()
    {
        if (fSM.parameters.cancelAttack)
        {
            fSM.ChangeState(TreeManStateType.Idle);
        }
        if (fSM.parameters.saplingIns == null)
            return;
        if (fSM.parameters.saplingIns.GetComponent<TreeManSapling>().impacted)
        {
            fSM.parameters.growPosition = fSM.parameters.saplingIns.transform.position;
            UnityEngine.Object.Destroy(fSM.parameters.saplingIns);
            fSM.ChangeState(TreeManStateType.Grow);
        }
    }

    public void OnUpdate()
    {
    }
}
