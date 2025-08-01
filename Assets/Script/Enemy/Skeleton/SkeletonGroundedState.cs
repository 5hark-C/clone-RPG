﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{

    protected Enemy_Skeleton enemy;
    protected Transform player;

    public SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(enemy.IsPlayerDetected()||Vector2.Distance(enemy.transform.position,player.position) < enemy.angerDistance)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
