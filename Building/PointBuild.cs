using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBuild : BuildMode {

    protected BuildSnapping buildSnapping;

    public PointBuild() 
        : base()
    {
        canRotate = true;
        canRotateSideways = true;
        canAlignRotation = true;
    }

    public override void update()
    {
        base.update();

        if (!buildSnapping) return;

        buildSnapping.update();

        if (buildSnapping.isSnapping())
            WhileSnapping();
    }

    public override GameObject prepare()
    {
        base.prepare();

        Renderer renderer = spawnedObj.GetComponent<Renderer>(); ;
        if (renderer)
            renderer.material = activeInvalidMaterial;

        buildSnapping = spawnedObj.AddComponent<BuildSnapping>();

        return spawnedObj;
    }

    protected override void modeReset()
    {
        Destroy(buildSnapping);
        base.modeReset();
    }

    public override void resetSpawnedObject()
    {
        base.resetSpawnedObject();

        if (buildSnapping)
            Destroy(buildSnapping);
    }

    public override void OnActiveRayContact(RaycastHit hit)
    {
        base.OnActiveRayContact(hit);

        spawnedObjRenderer.material = activeInvalidMaterial;
        canPlace = false;
    }

    protected void WhileSnapping()
    {
        spawnedObjRenderer.material = activeValidMaterial;
        canPlace = true;
        onPlace();
    }
}
