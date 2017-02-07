using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum BuildModeFlags
{
    FREE = 1,
    POINTBASED = 2
}

[System.Flags]
public enum BuildItemType
{
    NONE = 1,
    BASEPLATFORM = 2,
    WALL = 3,
    PILLAR = 4
}

class BuildItem : Item
{
    [EnumFlag]
    public BuildModeFlags buildModeFlags;
    [EnumFlag]
    public BuildModeFlags supportedBuildModeFlags;
    [EnumFlag]
    public BuildItemType buildItemTypeFlags;

    private GameObject player;

    public override bool OnUse(GameObject playerObj)
    {
        BuildingSystem buildingSystem = playerObj.GetComponent<BuildingSystem>();
        buildingSystem.build(gameObject);
        return true;
    }

    public override bool OnStopUse(GameObject playerObj)
    {
        BuildingSystem buildingSystem = playerObj.GetComponent<BuildingSystem>();
        buildingSystem.buildmode.interrupt();

        return true;
    }
}
