using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private float returnSpeed = 12;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRoate = true;
    private bool isReturning;

    private float freezeTimeDuration;

    [Header("Pierce info")]
    private float pierceAmount;

    [Header("Bounce info")]
    private float bounceSpeed;
    private bool isBouncing;
    private int bounceAmount;
    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Spin info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;

    private float spinDirection;


    private void Awake()
    {
        anim= GetComponentInChildren<Animator>();
        rb= GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void DestoryMe()
    {
        Destroy(gameObject);
    }

    public void SetupSword(Vector2 _dir,float _gravityScale,Player _player,float _freezeTimeDuration,float _returnSpeed)
    {
        player = _player;
        returnSpeed = _returnSpeed;
        freezeTimeDuration = _freezeTimeDuration;
        rb.velocity = _dir;
        rb.gravityScale= _gravityScale;

        if(pierceAmount <= 0)
            anim.SetBool("Rotation", true);

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestoryMe", 7);
        
    }

    public void SetupBounce(bool _isBouncing,int _amountOfBounce,float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounce;
        bounceSpeed = _bounceSpeed;

        enemyTarget = new List<Transform>();
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;

        
    }

    public void SetupSpin(bool _isSpinning,float _maxTravelDistance,float _spinDuration,float _hitCooldown)
    {
        spinDuration = _spinDuration;
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        hitCooldown = _hitCooldown;
    }

    private void Update()
    {
        if (canRoate)
        {
            transform.right = rb.velocity;
        }

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchSword();
            }
        }

        BounceLogic();
        SpinLogic();

    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

               // transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            SwordSkillDamger(hit.GetComponent<Enemy>());
                    }

                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                SwordSkillDamger(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamger(enemy);
        }



        SetupTargetsForBounce(collision);

        StuckInto(collision);

    }

    private void SwordSkillDamger(Enemy enemy)
    {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();

        player.stats.DoDamage(enemyStats);

        if(player.skill.sword.timeStopUnlocked)
            enemy.FreezeTimeFor(freezeTimeDuration);

        if (player.skill.sword.vulnurableUnlocked)
            enemyStats.MakeVulnerableFor(freezeTimeDuration + 5);
        

        ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);
        if (equipedAmulet != null)
            equipedAmulet.Effect(enemy.transform);
    }

    private void SetupTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform);
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            if (!wasStopped)
            {
            StopWhenSpinning();
            }
            return;
        }


        canRoate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
            return;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
