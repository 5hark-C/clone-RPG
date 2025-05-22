using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Freeze enemy effect", menuName = "Date/Item effect/Freeze enemy effect")]
public class FreezeEnemyEffect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * .1f)
            return;

        if (Inventory.instance.CanUseArmor())
            return;

           Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);

           foreach (var hit in colliders)
           {
              if (hit.GetComponent<Enemy>() != null)
              {
                hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
              }
           }

        
    }
}
