using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerIdleState : EnemyState
{
    private Enemy_DeathBringer enemy;
    private Transform player;

    public DeathBringerIdleState(Enemy _enemyBase, EnemyStateMachine stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(Vector2.Distance(player.transform.position, enemy.transform.position) < 7)
            enemy.bossFightBegun = true;

        if(Input.GetKeyUp(KeyCode.V))
        {
            stateMachine.ChangeState(enemy.teleportState);
        }

        if(stateTimer < 0 && enemy.bossFightBegun)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
