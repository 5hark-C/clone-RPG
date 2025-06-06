using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeathBringerTrigger : Enemy_AnimationTrigger
{
    private Enemy_DeathBringer enemyDeathBring => GetComponentInParent<Enemy_DeathBringer>();

    private void Relocate() => enemyDeathBring.FindPosition();

    private void MakeInvisible() => enemyDeathBring.fx.MakeTransprent(true);

    private void Makevisible() => enemyDeathBring.fx.MakeTransprent(false);
}
