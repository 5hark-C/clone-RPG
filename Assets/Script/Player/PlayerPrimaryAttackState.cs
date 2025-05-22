using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter {  get; private set; }

    private float lastTimeAttacked;
    private float comboWindow = 0.5f;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachin _stateMachin, string _animBoolName) : base(_player, _stateMachin, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //AudioManager.instance.PlaySFX(2); // 攻击音效

        xInput = 0;//修复攻击方向bug

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }

        player.anim.SetInteger("ComboCounter", comboCounter);

        #region Choose attack direction
        float attackDir = player.facingDir;
        if (xInput != 0)
        {
            attackDir = xInput;
        }
        #endregion

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor",.15f);

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            player.SetZeroVelocity();
        }


        if (triggerCalled)
        {
            stateMachin.ChangeState(player.idleState);
        }
    }
}
