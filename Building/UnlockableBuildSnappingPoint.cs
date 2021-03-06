﻿using System.Collections;
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
        if (isRequiredPointsOccupied())
        {
            if (!unlocked)
                unlockPoint();
        } else if (unlocked)
        {
            lockPoint();
        }

        if (isCorrectType(other))
            return true;

        return true;
    }

    private bool isRequiredPointsOccupied()
    {
        foreach (GameObject snapPoint in requiredSnappingPoints)
        {
            BuildSnappingPoint point = snapPoint.GetComponent<BuildSnappingPoint>();
            if (!point.isOccupied())
                return false;
        }

        return true;

    }

    private void lockPoint()
    {
        buildItemTypeFlags &= ~(typesToUnlock);
        buildItemTypeFlags |= BuildItemType.NONE;

        unlocked = false;
    }

    private void unlockPoint()
    {
        if (FlagHelper.IsSet(buildItemTypeFlags, BuildItemType.NONE))
            buildItemTypeFlags &= ~(BuildItemType.NONE);

        buildItemTypeFlags |= typesToUnlock;

        unlocked = true;
    }
}
