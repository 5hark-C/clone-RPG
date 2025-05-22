using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachin _stateMachin, string _animBoolName) : base(_player, _stateMachin, _animBoolName)
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachin.ChangeState(player.wallJumpState);
            return;
        }

        if (xInput != 0 && player.facingDir != xInput)
        {
            stateMachin.ChangeState(player.idleState);
        }

        if (yInput < 0)
        {
            rb.velocity=new Vector2(0,rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y * .7f);
        }




        if(player.IsGroundDetected())
        {
            stateMachin.ChangeState(player.idleState);
        }
    }
}
