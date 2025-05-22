using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackholeSkill : Skills
{
    [SerializeField] private UI_SkilltreeSlot blackHoleUnlockButtom;
    public bool blackholeUnlocked {  get; private set; }
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCooldown;
    [SerializeField] private float blackholeDuration;
    [Space]
    [SerializeField] private GameObject balckHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;


    BlackholeSkillController currentBlackhole;


    private void UnlockBlackhole()
    {
        if (blackHoleUnlockButtom.unlocked)
            blackholeUnlocked = true;
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(balckHolePrefab,player.transform.position,Quaternion.identity);

        currentBlackhole = newBlackHole.GetComponent<BlackholeSkillController>();

        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCooldown,blackholeDuration);

        AudioManager.instance.PlaySFX(6, player.transform);
        AudioManager.instance.PlaySFX(3, player.transform);
    }

    protected override void Start()
    {
        base.Start();

        blackHoleUnlockButtom.GetComponent<Button>().onClick.AddListener(UnlockBlackhole);
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillCompleted()
    {
        if(!currentBlackhole)
            return false;

        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }

        return false;
    }

    
    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }

    protected override void CheckUnlock()
    {
        base.CheckUnlock();
        UnlockBlackhole();
    }

}
