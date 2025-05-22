using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skills
{

    [Header("Dash")]
    [SerializeField] private UI_SkilltreeSlot dashUnlockButton;
    public bool dashUnlocked {  get; private set; }

    [Header("Clone on dash")]
    [SerializeField] private UI_SkilltreeSlot cloneOnDashUnlockButton;
    public bool cloneOnDashUnlocked { get; private set; }

    [Header("Clone on arrival")]
    [SerializeField] private UI_SkilltreeSlot cloneOnArrivalUnlockButton;
    public bool cloneOnArrivalUnlocked {  get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();
        AudioManager.instance.PlaySFX(36,null);
        Debug.Log("Creat clone behind");
    }

    protected override void Start()
    {
        base.Start();

        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    protected override void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneOnDash();
        UnlockCloneOnArrival();
    }

    private void UnlockDash()
    {
        if(dashUnlockButton.unlocked)
           dashUnlocked = true;
    }

    private void UnlockCloneOnDash()
    {
        if(cloneOnDashUnlockButton.unlocked)
           cloneOnDashUnlocked = true;
    }

    private void UnlockCloneOnArrival()
    {
        if(cloneOnArrivalUnlockButton.unlocked)
           cloneOnArrivalUnlocked = true;
    }

    //在冲刺起点创造克隆攻击
    public void CloneOnDash()
    {
        if (cloneOnDashUnlocked)
            SkillManger.instance.clone.CreateClone(player.transform, Vector3.zero);
    }


    //在冲刺终点创造克隆攻击
    public void CloneOnArrival()
    {
        if (cloneOnArrivalUnlocked)
            SkillManger.instance.clone.CreateClone(player.transform, Vector3.zero);
    }
}
