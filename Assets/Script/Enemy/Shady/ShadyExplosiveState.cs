using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyExplosiveState : EnemyState
{
    private Enemy_Shady enemy;

    public ShadyExplosiveState(Enemy _enemyBase, EnemyStateMachine stateMachine, string _animBoolName,Enemy_Shady _enemy) : base(_enemyBase, stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled)
            enemy.SelfDestroy();
    }

}
