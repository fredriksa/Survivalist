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

        newPos.x = activePoint.transform.position.x;
        newPos.y = activePoint.transform.position.y + ObjectHelper.getSizeFromRenderer(gameObject).y / 2;
        newPos.z = activePoint.transform.position.z;

        gameObject.transform.position = newPos;
        gameObject.transform.rotation = activePoint.transform.rotation;

        Vector3 size = ObjectHelper.getSizeFromRenderer(gameObject.gameObject);
        float dist = size.x / 2;
        gameObject.transform.Translate(activePoint.modifier * dist);
    }
}
