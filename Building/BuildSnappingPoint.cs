using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSnappingPoint : MonoBehaviour
{
    //Vector3 forward, left, right etc.
    public Vector3 modifier = Vector3.one;
    public GameObject snappedObject;

    private void OnTriggerEnter(Collider other)
    {
        BuildSnapping snapping = other.gameObject.GetComponent<BuildSnapping>();
        if (!snapping || isOccupied()) return;

        snappedObject = other.gameObject;
        snapping.activePoint = this;
    }

    private void OnC(Collider other)
    {
        BuildSnapping snapping = other.gameObject.GetComponent<BuildSnapping>();
        if (!snapping) return;

        snappedObject = other.gameObject;
        snapping.activePoint = null;
    }

    public bool isOccupied()
    {
        return snappedObject != null;
    }
}
