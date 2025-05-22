using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachin _stateMachin, string _animBoolName) : base(_player, _stateMachin, _animBoolName)
    {
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

        if(player.IsWallDetected())
        {
            stateMachin.ChangeState(player.wallSlideState);
        }

        if (player.IsGroundDetected())
        {
            stateMachin.ChangeState(player.idleState);
        }

        if (xInput != 0)
        {
            player.SetVelocity(player.moveSpeed * .8f * xInput, rb.velocity.y);
        }
    }
}
