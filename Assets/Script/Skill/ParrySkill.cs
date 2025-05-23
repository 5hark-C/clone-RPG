using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skills
{
    [Header("Parry")]
    [SerializeField] private UI_SkilltreeSlot parryUnlockButton;
    public bool parryUnlocked {  get; private set; }

    [Header("Parry restore")]
    [SerializeField] private UI_SkilltreeSlot restoreUnlockButton;
    public bool restoreUnlocked { get; private set; }
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthPerentage;

    [Header("Parry with mirage")]
    [SerializeField] private UI_SkilltreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked {  get; private set; }


    public override void UseSkill()
    {
        base.UseSkill();

        if (restoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthPerentage);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockParryRestore();
        UnlockParryWithMirage();
    }

    private void UnlockParry()
    {
        if(parryUnlockButton.unlocked)
            parryUnlocked = true;
    }

    private void UnlockParryRestore()
    {
        if(restoreUnlockButton.unlocked)
            restoreUnlocked = true;
    }

    private void UnlockParryWithMirage()
    {
        if(parryWithMirageUnlockButton.unlocked)
            parryWithMirageUnlocked = true;
    }

    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (parryWithMirageUnlocked)
            SkillManger.instance.clone.CreateCloneWithDelay(_respawnTransform);
    }
}
