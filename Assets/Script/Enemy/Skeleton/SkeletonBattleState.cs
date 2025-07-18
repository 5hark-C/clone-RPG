﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private Enemy_Skeleton enemy;
    private int moveDir;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void Update()
    {
        base.Update();

        if(enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if(enemy.IsPlayerDetected().distance<enemy.attackDistance)
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
            }
        }
        else
        {
            if (stateTimer < 0||Vector2.Distance(player.transform.position,enemy.transform.position)>7)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        if(player.position.x>enemy.transform.position.x)
        {
            moveDir = 1;
        }else if(player.position.x<enemy.transform.position.x)
        {
            moveDir = -1;
        }

        if (enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance < enemy.attackDistance - .1f)
            return;

        enemy.SetVelocity(enemy.moveSpeed * moveDir,rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if(Time.time>=enemy.lastTimeAttacked+enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minattackCooldown, enemy.maxattackCooldown);
            enemy.lastTimeAttacked=Time.time;
            return true;
        }
        return false;
    }
}
