using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skills
{
    [Header("Dodge")]
    [SerializeField] private UI_SkilltreeSlot unlockDodgeButton;
    [SerializeField] private int evasionAmounet;
    public bool dodgeUnlocked;

    [Header("Mirage dodge")]
    [SerializeField] protected UI_SkilltreeSlot unlockMirageDodgeButton;
    public bool dodgeMirageUnlocked;

    protected override void Start()
    {
        base.Start();

        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }

    protected override void CheckUnlock()
    {
        UnlockDodge();
        UnlockMirageDodge();
    }

    private void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked && !dodgeUnlocked)
        {
            player.stats.evasion.AddModifier(evasionAmounet);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }

    private void UnlockMirageDodge()
    {
        if (unlockDodgeButton.unlocked)
            dodgeMirageUnlocked = true;
        
    }

    public void CreateMirageOnDodge()
    {
        if(dodgeMirageUnlocked)
            SkillManger.instance.clone.CreateClone(player.transform,new Vector3(2 * player.facingDir,0));
    }
}
