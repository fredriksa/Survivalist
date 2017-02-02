using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ItemActivateHandler {

    static public void OnActivate(Item item, GameObject player)
    {
        item.OnActivate(player);
    }
}
