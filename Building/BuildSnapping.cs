using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSnapping : MonoBehaviour
{
    public BuildSnappingPoint activePoint = null;

    public void update()
    {
        if (!isSnapping()) return;

        moveToSnappingPoint();
    }

    public bool isSnapping()
    {
        return activePoint != null;
    }

    private void moveToSnappingPoint()
    {
        Vector3 newPos = Vector3.zero;

        Transform positionSource = getPositionSource();

        newPos.x = positionSource.position.x;
        newPos.y = positionSource.position.y + ObjectHelper.getSizeFromRenderer(gameObject).y / 2;
        newPos.z = positionSource.position.z;

        gameObject.transform.position = newPos;
        gameObject.transform.rotation = positionSource.rotation;

        Vector3 size = ObjectHelper.getSizeFromRenderer(gameObject.gameObject);
        float dist = size.x / 2;
        gameObject.transform.Translate(activePoint.modifier * dist);
    }

    private Transform getPositionSource()
    {
        if (activePoint.snapPoint)
            return activePoint.snapPoint.transform;

        return activePoint.transform;
    }
}
