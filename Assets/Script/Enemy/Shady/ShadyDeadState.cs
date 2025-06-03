using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyDeadState : EnemyState
{
    public ShadyDeadState(Enemy _enemyBase, EnemyStateMachine stateMachine, string _animBoolName) : base(_enemyBase, stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();


    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
