using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SlotContainer))]
public class Inventory : MonoBehaviour {
    public SlotContainer slots;
    public GameObject itemDatabaseObj;

    private Database itemDatabase;

    void Start()
    {
        slots = GetComponent<SlotContainer>();
        
        //Remove below when testing done
        itemDatabase = itemDatabaseObj.GetComponent<Database>();

        addItem(itemDatabase.fetch(0).GetComponent<Item>(), 3);
        addItem(itemDatabase.fetch(1).GetComponent<Item>(), 2);
        /*addItem(itemDatabase.fetch(0).GetComponent<Item>(), 3);
        addItem(itemDatabase.fetch(1).GetComponent<Item>(), 32);
        addItem(itemDatabase.fetch(1).GetComponent<Item>(), 32);
        addItem(itemDatabase.fetch(1).GetComponent<Item>(), 32);
        removeItem(itemDatabase.fetch(0).GetComponent<Item>(), 6);
        addItem(itemDatabase.fetch(1).GetComponent<Item>(), 32);*/
    }

    public bool addItem(Item item, int amount = 1)
    {
        return slots.distributeItem(item, amount);
    }

    public bool removeItem(Item item, int amount = 1)
    {
        return slots.removeItem(item, amount);
    }

    public bool hasItem(Item item, int amount = 1)
    {
        int count = slots.getItemCount(item);
        return  count >= amount;
    }
}
