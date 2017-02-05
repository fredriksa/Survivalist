using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableBuildSnappingPoint : BuildSnappingPoint {

    public List<GameObject> requiredSnappingPoints;
    [EnumFlag]
    public BuildItemType typesToUnlock;

    private bool unlocked = false;

    public override bool canSnap(Collider other)
    {
        if (!base.canSnap(other))
            return false;

        if (!hasRequirements(other))
            return false;

        return true;
    }

    private bool hasRequirements(Collider other)
    {
        if (isCorrectType(other))
            return true;

        foreach (GameObject snapPoint in requiredSnappingPoints)
        {
            BuildSnappingPoint point = snapPoint.GetComponent<BuildSnappingPoint>();
            if (!point.isOccupied())
                return false;
        }

        if (!unlocked)
        {
            if (FlagHelper.IsSet(buildItemTypeFlags, BuildItemType.NONE))
                buildItemTypeFlags &= ~(BuildItemType.NONE);

            buildItemTypeFlags |= typesToUnlock;

            unlocked = true;
        }

        return true;
    }
}
