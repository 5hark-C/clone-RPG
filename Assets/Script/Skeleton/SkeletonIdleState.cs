﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine stateMachine, string _animBoolName, Enemy_Skeleton enemy) : base(_enemyBase, stateMachine, _animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.instance.PlaySFX(24,enemy.transform);
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer<0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}
