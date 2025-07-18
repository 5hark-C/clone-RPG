﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AnimationTrigger : MonoBehaviour
{
    private Enemy enemy=>GetComponentInParent<Enemy>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Player>()!= null)
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(target);

                
            }
        }
    }

    private void SpeicalAttackTrigger()
    {
        enemy.AnimationSpecialAttackTrigger();
    }

    private void OpenCounterAttackWindow() => enemy.OpenCounterAttackWinndow();
    private void CloseCountrtAttackWindow()=>enemy.CloseCounterAttackWindou();
}

