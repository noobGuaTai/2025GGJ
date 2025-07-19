using System;
using System.Linq;
using UnityEngine;

public class TreeManAttackState : IState
{
    private TreeManFSM fsm;

    public TreeManAttackState(TreeManFSM fSM)
    {
        this.fsm = fSM;
    }
    public GameObject Shoot(Vector3 targetPos)
    {
        Debug.Log($"Shoot {Time.realtimeSinceStartup}");
        fsm.attackAudio.Play();
        var saplingIns = UnityEngine.Object.Instantiate(fsm.parameters.saplingPrefab);
        saplingIns.transform.position = fsm.transform.position;
        saplingIns.transform.parent = fsm.transform;
        var rb = saplingIns.GetComponent<Rigidbody2D>();
        for (var angle = 30; angle < 360; angle += 30)
        {
            if (angle % 90 == 0) continue;
            var v = ProjectileMotion.CalculateInitialVelocity(
                fsm.transform.position, targetPos, angle,
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
        fsm.animator.Play("attack", 0, 0);
        fsm.parameters.cancelAttack = false;
    }

    public void OnExit()
    {
        UnityEngine.Object.Destroy(fsm.parameters.saplingIns);
        fsm.parameters.saplingIns = null;
    }

    public void OnFixedUpdate()
    {
        if (fsm.parameters.cancelAttack)
        {
            fsm.ChangeState(TreeManStateType.Idle);
        }
        if (fsm.parameters.saplingIns == null)
            return;
        if (fsm.parameters.saplingIns.GetComponent<TreeManSapling>().impacted)
        {
            fsm.parameters.growPosition = fsm.parameters.saplingIns.transform.position;
            UnityEngine.Object.Destroy(fsm.parameters.saplingIns);
            fsm.ChangeState(TreeManStateType.Grow);
        }
    }

    public void OnUpdate()
    {
    }
}
