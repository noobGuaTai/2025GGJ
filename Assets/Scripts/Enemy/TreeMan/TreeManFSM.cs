using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TreeManStateType
{
    Idle,
    Grow,
    Attack
}

[Serializable]
public class TreeManParameters
{
    public TreeManStateType currentState;
    public Vector3 growPosition;
    public GameObject saplingPrefab;
    public GameObject saplingGrowUpMirage;
    public GameObject saplingIns = null;
    public bool cancelAttack = false;
    public AudioSource growAudio;
}

public class TreeManFSM : EnemyFSM
{
    public void AnimationEventOnShoot()
    {
        var attackState = state[TreeManStateType.Attack] as TreeManAttackState;
        var targets = GetComponent<TargetCollect>().attackTarget;
        if (targets.Count != 0)
            parameters.saplingIns = attackState.Shoot(targets.First().transform.position);
        else parameters.cancelAttack = true;
    }
    public TreeManParameters parameters;
    public IState currentState;
    public Dictionary<TreeManStateType, IState> state = new Dictionary<TreeManStateType, IState>();
    public void TreeManGrowFinish()
    {
        (state[TreeManStateType.Grow] as TreeManGrowState).growFinish = true;
    }

    public override void Start()
    {
        base.Start();
        state.Add(TreeManStateType.Idle, new TreeManIdleState(this));
        state.Add(TreeManStateType.Grow, new TreeManGrowState(this));
        state.Add(TreeManStateType.Attack, new TreeManAttackState(this));
        ChangeState(TreeManStateType.Idle);
    }

    void Update()
    {
        currentState.OnUpdate();
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public void ChangeState(TreeManStateType stateType)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = state[stateType];
        currentState.OnEnter();
        parameters.currentState = stateType;
    }
}
