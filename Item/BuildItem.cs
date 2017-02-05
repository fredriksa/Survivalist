using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum BuildModeFlags
{
    FREE = 1,
    POINTBASED = 2
}

class BuildItem : Item
{
    [EnumFlag]
    public BuildModeFlags buildModeFlags;
    [EnumFlag]
    public BuildModeFlags supportedBuildModeFlags;

    private GameObject player;

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
