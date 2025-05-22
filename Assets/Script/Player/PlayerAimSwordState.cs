﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    
    public PlayerAimSwordState(Player _player, PlayerStateMachin _stateMachin, string _animBoolName) : base(_player, _stateMachin, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
         
        player.skill.sword.DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        
        
        player.StartCoroutine("BusyFor", .1f);
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            stateMachin.ChangeState(player.idleState);
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(player.transform.position.x > mousePosition.x && player.facingDir == 1)
        {
            player.Flip();
        }else if(player.transform.position.x < mousePosition.x && player.facingDir == -1)
        {
            player.Flip();
        }


    }
}
