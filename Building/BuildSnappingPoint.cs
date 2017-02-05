using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSnappingPoint : MonoBehaviour
{
    //Vector3 forward, left, right etc.
    public Vector3 modifier = Vector3.one;
    public GameObject snappedObject;

    [EnumFlag]
    public BuildItemType buildItemTypeFlags;

    private void OnTriggerEnter(Collider other)
    {
        if (!canSnap(other) || !isCorrectType(other))
            return;

        BuildSnapping snapping = other.gameObject.GetComponent<BuildSnapping>();
        snappedObject = other.gameObject;
        snapping.activePoint = this;
    }

    /*private void OnTriggerExit(Collider other)
    {
        BuildSnapping snapping = other.gameObject.GetComponent<BuildSnapping>();
        if (!snapping) return;

        snappedObject = other.gameObject;
        snapping.activePoint = null;
    }*/

    public bool isCorrectType(Collider other)
    {
        BuildItem item = other.GetComponent<BuildItem>();
        if (!item) return false;

        if (buildItemTypeFlags == 0)
            return true;

        BuildItemType flagResult = item.buildItemTypeFlags & buildItemTypeFlags;
        if (flagResult <= 0)
            return false;

        return true;
    }

    public virtual bool canSnap(Collider other)
    {
        BuildSnapping snapping = other.gameObject.GetComponent<BuildSnapping>();
        if (!snapping || isOccupied() || snapping.isSnapping())
            return false;

        return true;
    }

    public bool isOccupied()
    {
        return snappedObject != null;
    }
}
