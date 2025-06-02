using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherDeadState : EnemyState
{
    private Enemy_Archer enemy;

    public ArcherDeadState(Enemy _enemyBase, EnemyStateMachine stateMachine, string _animBoolName) : base(_enemyBase, stateMachine, _animBoolName)
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
