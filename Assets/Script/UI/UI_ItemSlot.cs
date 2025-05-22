using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    protected UI ui;
    public InventoryItem item;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem _newitem)
    {
        item = _newitem;
        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void ClearUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        //删除装备
        if (Input.GetKey(KeyCode.Delete))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        if (item.data.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.data);

        ui.itemToolTip.HideTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item == null) 
            return;

        ui.itemToolTip.ShowTooltip(item.data as ItemData_Equipment);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
            return;

        ui.itemToolTip.HideTooltip();
    }
}
