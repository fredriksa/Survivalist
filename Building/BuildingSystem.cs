using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour {
    public Camera headCamera;
    public KeyCode placementKey = KeyCode.Mouse0;
    public KeyCode alignRotationKey = KeyCode.Mouse1;
    public KeyCode rotationModifierKey = KeyCode.LeftControl;
    public KeyCode heightModifierKey = KeyCode.LeftShift;
    public float placementDistance = 7;
    public float rotationSpeed = 3;

    public Material activeValidMaterial;
    public Material activeInvalidMaterial;

    private BuildMode buildMode = new FreeBuild();

	void Start ()
    {
        if (!headCamera)
            Debug.LogWarning("BuildingSystem: Camera is not set for " + gameObject.name);

        if (!activeValidMaterial)
            Debug.LogWarning("BuildingSystem: Active material is not set for " + gameObject.name);

        prepareMaterials();
    }

    private void prepareMaterials()
    {
        Color color = activeValidMaterial.color;
        color.a = 0.6f;
        activeValidMaterial.color = color;

        color = activeInvalidMaterial.color;
        color.a = 0.6f;
        activeInvalidMaterial.color = color;
    }

    private void setBuildMode(GameObject prefab)
    {
        if (FlagHelper.IsSet(prefab.GetComponent<BuildItem>().buildModeFlags, BuildModeFlags.FREE))
            buildMode = new FreeBuild();
        else if (FlagHelper.IsSet(prefab.GetComponent<BuildItem>().buildModeFlags, BuildModeFlags.POINTBASED))
            buildMode = new PointBuild();

        buildMode.activeInvalidMaterial = activeInvalidMaterial;
        buildMode.activeValidMaterial = activeValidMaterial;
        buildMode.placementDistance = placementDistance;
        buildMode.headCamera = headCamera;
        buildMode.rotationSpeed = rotationSpeed;
    }

    void Update ()
    {
        buildMode.update();
    }

    public void build(GameObject obj)
    {
        if (buildMode.isBuilding())
            buildMode.destroyActiveObject();

        setBuildMode(obj);
        buildMode.spawn(obj, transform.position, transform.rotation);
        buildMode.prepare();
    }
}
