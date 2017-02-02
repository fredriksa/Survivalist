using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FoodItem : Item
{
    public int heal = 10;

    public override bool OnUse(GameObject player)
    {
        Health health = player.GetComponent<Health>();
        if (!health) return false;

        health.heal(heal);

        player.GetComponent<Inventory>().removeItem(this);

        return true;
    }
}
