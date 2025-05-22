using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject_Trigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();

    //拾取逻辑
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<Player>() != null)
        {
            if (collision.GetComponent<CharacterStats>().isDead)
                return;

            myItemObject.PickUpItem();

        }
    }
}
