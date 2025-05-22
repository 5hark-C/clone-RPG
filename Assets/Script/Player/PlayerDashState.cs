using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachin _stateMachin, string _animBoolName) : base(_player, _stateMachin, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.dash.CloneOnDash();

        stateTimer = player.dashDuration;
        player.isInvincible = true;
    }

    public override void Exit()
    {
        base.Exit();


        player.skill.dash.CloneOnArrival();
        player.SetVelocity(0, rb.velocity.y);
        player.isInvincible = false;
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected()&&player.IsWallDetected())
        {
            stateMachin.ChangeState(player.wallSlideState);
        }
   
        player.SetVelocity(player.dashSpeed * player.dashDir,0);

        if (stateTimer < 0) 
        {
            stateMachin.ChangeState(player.idleState);
        }
    }
}
    

