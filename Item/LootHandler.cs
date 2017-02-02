using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class LootHandler : MonoBehaviour
{
    static public void OnLoot(GameObject item, GameObject player)
    {
        Inventory inventory = player.GetComponent<Inventory>();
        if (!inventory) return;


        Database itemDatabase = GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<Database>();
        //We need to reference the prefab in the database as we're shortly after destroying the item gameobject
        GameObject targetItem = itemDatabase.fetch(item.GetComponent<Item>().getEntry());
        inventory.addItem(targetItem.GetComponent<Item>());
        UIHandler.Instance.announceEvent("O B T A I N E D " + targetItem.GetComponent<Item>().itemName);
        Destroy(item);
    }
}
