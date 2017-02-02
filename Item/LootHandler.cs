using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class LootHandler : MonoBehaviour
{
    static public void OnLoot(Item item, GameObject player)
    {
        Inventory inventory = player.GetComponent<Inventory>();
        if (!inventory) return;

        Database itemDatabase = GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<Database>();
        int itemEntry = 2;//item.getEntry();
        GameObject targetItem = itemDatabase.fetch(itemEntry);
        inventory.addItem(targetItem.GetComponent<Item>());
        Destroy(item.gameObject);

        UIHandler.Instance.announce("Looted " + item.itemName);
    }
}
