using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BuildItem : Item
{
    private GameObject player;

    /*public void Update()
    {
        Debug.Log(x);
        if (player == null || spawnedObj == null || playerCamera == null) return;

        Vector3 pos = playerCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, playerCamera.nearClipPlane));

        spawnedObj.transform.position = pos;
    }*/

    public override bool OnUse(GameObject playerObj)
    {
        BuildingSystem buildingSystem = playerObj.GetComponent<BuildingSystem>();
        buildingSystem.build(gameObject);
        return true;
    }

    public override bool OnStopUse(GameObject playerObj)
    {
        //Implement functionality so this "hook" gets triggered
        return true;
    }
}
