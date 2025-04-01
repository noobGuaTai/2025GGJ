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
        saplingIns = Shoot(fSM.GetComponent<TargetCollect>().attackTarget.First().transform.position);
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
        if (saplingIns == null)
        {
            fSM.ChangeState(TreeManStateType.Idle);
            return;
        }
        if (saplingIns.GetComponent<TreeManSapling>().impacted)
        {
            fSM.parameters.growPosition = saplingIns.transform.position;
            UnityEngine.Object.Destroy(saplingIns);
            fSM.ChangeState(TreeManStateType.Grow);
        }
    }

    public void OnUpdate()
    {
    }
}
