﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private Player player;
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    private float crystalExistTimer;

    private bool caanExplode;
    private bool canMove;
    private float moveSpeed;

    private Transform closestTarget;

    private bool canGrow;
    [SerializeField] private float growSpeed = 5;

    [SerializeField] private LayerMask whatIsEnemy;

    public void SetupCrystal(float _crystalDuration,bool _canExplode,bool _canMove,float _moveSpeed,Transform _closestTarget,Player _player)
    {
        crystalExistTimer = _crystalDuration;
        caanExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
        player = _player;
    }


    //随机索敌函数
    public void ChooseRandomEnemy()
    {
        float radius = SkillManger.instance.blackhole.GetBlackholeRadius();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,radius,whatIsEnemy);

        if(colliders.Length > 0)
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;

        if(crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if (canMove)
        {
            if (closestTarget == null)
                return;

            //寻敌
            transform.position = Vector2.MoveTowards(transform.position,closestTarget.position,moveSpeed *  Time.deltaTime);

            //距离检测（如果小于1自爆）
            if (Vector2.Distance(transform.position, closestTarget.position) < 1)
            {
                FinishCrystal();
                canMove = false;
            }

        }

        if(canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale,new Vector2(3,3),growSpeed * Time.deltaTime);
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());

                ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);
                if (equipedAmulet != null)
                    equipedAmulet.Effect(hit.transform);
            }
        }
    }

    //水晶行为逻辑（爆炸，自毁）
    public void FinishCrystal()
    {
        if (caanExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
            SelfDestroy();
    }

    public void SelfDestroy() => Destroy(gameObject);
}
