using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ItemHandler {

    static private Item lastActivatedItem = null;

    static public void OnActive(Item item, GameObject player)
    {
        if (lastActivatedItem)
            lastActivatedItem.OnInactive(player);

        item.OnActivate(player);
        lastActivatedItem = item;
    }
}
