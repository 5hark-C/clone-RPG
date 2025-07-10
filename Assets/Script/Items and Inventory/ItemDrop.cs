using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int maxItemToDrop;
    [SerializeField] private ItemData[] itemPool;
    private List<ItemData> possibleDrop = new List<ItemData>();

    [SerializeField] private GameObject dropPerfab;


    //随机掉落逻辑
    public virtual void GenerateDrop()
    {
        if(itemPool.Length == 0)
        {
            return;
        }

        foreach(ItemData item in itemPool)
        {
            if(item != null && Random.Range(0,100) < item.dropChance)
            {
                possibleDrop.Add(item);
            }
        }

        for(int i= 0; i < maxItemToDrop; i++)
        {
            if(possibleDrop.Count > 0)
            {
                int randomIndex = Random.Range(0, possibleDrop.Count);
                ItemData itemToDrop = possibleDrop[randomIndex];

                DropItem(itemToDrop);
                possibleDrop.Remove(itemToDrop);
            }
        }
    }

    protected void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPerfab,transform.position,Quaternion.identity);
        Vector2 randomVelocity = new Vector2(Random.Range(-5,5),Random.Range(15,20));

        newDrop.GetComponent<ItemObject>().SetUpItem(_itemData,randomVelocity);
    }
}
