using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("玩家掉落")]
    [SerializeField] private float chanceToLoseItem;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;

        List<InventoryItem> materulsToLOse = new List<InventoryItem>();

        foreach (InventoryItem item in inventory.GetStashList())
        {
            if(Random.Range(0,100) <= chanceToLoseItem)
            {
                DropItem(item.data);
                materulsToLOse.Add(item);
            }
        }

        for (int i = 0; i < materulsToLOse.Count; i++)
        {
            inventory.RemoveItem(materulsToLOse[i].data);
        }
    }

}
