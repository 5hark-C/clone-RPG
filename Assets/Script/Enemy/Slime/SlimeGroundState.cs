using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGroundState : EnemyState
{
    protected Transform player;
    protected Enemy_Slime enemy;

    public SlimeGroundState(Enemy _enemyBase, EnemyStateMachine stateMachine, string _animBoolName,Enemy_Slime _enemy) : base(_enemyBase, stateMachine, _animBoolName)
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

        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < enemy.angerDistance)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
