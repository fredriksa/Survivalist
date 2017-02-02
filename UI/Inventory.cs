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
