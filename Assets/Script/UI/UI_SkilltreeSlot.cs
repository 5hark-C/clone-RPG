using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkilltreeSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,ISaveManager
{
    private UI ui;

    [SerializeField] private int skillCost;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedSkillColor;

    public bool unlocked;

    [SerializeField] private UI_SkilltreeSlot[] shouldBeUnlock;
    [SerializeField] private UI_SkilltreeSlot[] shouldBelock;

    private Image skillImage;


    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
        
    }

    private void Start()
    {
        ui = GetComponentInParent<UI>();
        skillImage = GetComponent<Image>();
        skillImage.color = lockedSkillColor;

        if (unlocked)
            skillImage.color = Color.white;
    }

    public void UnlockSkillSlot()
    {
        if (PlayerManager.instance.HaveEnoughMoney(skillCost) == false)
            return;

        for (int i = 0; i < shouldBeUnlock.Length; i++)
        {
            if (shouldBeUnlock[i].unlocked == false)
            {
                return;
            }
        }

        for (int i = 0; i < shouldBelock.Length; i++)
        {
            if (shouldBelock[i].unlocked == true)
            {
                return;
            }
        }

        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription,skillName,skillCost);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }

    public void LoadData(GameData _data)
    {
        if(_data.skillTree.TryGetValue(skillName,out bool value))
        {
            unlocked = value;
            Debug.Log($"Loading skill {skillName}, unlocked: {unlocked}");
        }
    }

    public void SaveData(ref GameData _data)
    {
        if(_data.skillTree.TryGetValue(skillName,out bool value))
        {
            _data.skillTree.Remove(skillName);
            _data.skillTree.Add(skillName, unlocked);
        }
        else
        {
            _data.skillTree.Add(skillName, unlocked);
        }
    }
}
