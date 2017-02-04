using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BuildSnappingPoint : MonoBehaviour
{
    //Vector3 forward, left, right etc.
    public Vector3 modifier = Vector3.one;
    public bool isOccupied = false;

    private void OnTriggerStay(Collider other)
    {
        BuildSnapping snapping = other.gameObject.GetComponent<BuildSnapping>();
        if (!snapping || isOccupied || snapping.hasSnapped) return;

        Vector3 newPos = Vector3.zero;

        newPos.x = transform.position.x;
        newPos.y = transform.position.y + ObjectHelper.getSizeFromRenderer(other.gameObject).y / 2;
        newPos.z = transform.position.z;

        other.transform.position = newPos;
        other.transform.rotation = transform.rotation;

        Vector3 size = ObjectHelper.getSizeFromRenderer(other.gameObject);
        float dist = size.x / 2;
        other.transform.Translate(modifier * dist);

        snapping.hasSnapped = true;
        isOccupied = true;
    }
}
