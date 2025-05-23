﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState :PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachin _stateMachin, string _animBoolName) : base(_player, _stateMachin, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (xInput == player.facingDir && player.IsWallDetected())
        {
            return;
        }

        if (xInput != 0 && !player.isBusy)
        {
            stateMachin.ChangeState(player.moveState);
        }

    }
}
