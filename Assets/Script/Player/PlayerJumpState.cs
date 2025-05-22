using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachin _stateMachin, string _animBoolName) : base(_player, _stateMachin, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.velocity = new Vector2(rb.velocity.x,player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(rb.velocity.y< 0)
        {
            stateMachin.ChangeState(player.airState);
        }
    }
}
