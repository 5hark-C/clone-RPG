using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skills : MonoBehaviour
{
    public float cooldown;
    public float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player=PlayerManager.instance.player;
        CheckUnlock();
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    protected virtual void CheckUnlock()
    {

    }

    public virtual bool CanUseSkill()
    {
        if(cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        else
        {
            player.fx.CreatePopUpText("Cooldown");
            return false;
        }
    }

    public virtual void UseSkill()
    {

    }
    //寻找最近敌人逻辑
    protected virtual Transform FindClosestEnemy(Transform _checkTransfrom)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransfrom.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToenemy = Vector2.Distance(_checkTransfrom.position, hit.transform.position);

                if (distanceToenemy < closestDistance)
                {
                    closestDistance = distanceToenemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        return closestEnemy;
    }
}
