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
    public float placementDistance = 4;
    public float rotationSpeed = 3;

    public Material activeValidMaterial;
    public Material activeInvalidMaterial;

    private BuildMode buildMode = new FreeBuild();
    private BuildModeFlags buildModeFlag = BuildModeFlags.FREE;
    private GameObject prefabObj;

	void Start ()
    {
        if (!headCamera)
            Debug.LogWarning("BuildingSystem: Camera is not set for " + gameObject.name);

        if (!activeValidMaterial)
            Debug.LogWarning("BuildingSystem: Active material is not set for " + gameObject.name);

        prepareMaterials();
    }


    void Update()
    {
        buildMode.update();

        if (Input.GetKeyDown(KeyCode.C))
            buildMode.interrupt();

        if (Input.GetKeyDown(KeyCode.X))
            toggleBuildMode();
    }


    public void build(GameObject obj)
    {
        buildMode.interrupt();
        setBuildMode(obj);
        buildMode.spawn(obj, transform.position, transform.rotation);
        buildMode.prepare();
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
        prefabObj = prefab;
        if (FlagHelper.IsSet(prefab.GetComponent<BuildItem>().buildModeFlags, BuildModeFlags.FREE))
        {
            buildMode = new FreeBuild();
            buildModeFlag = BuildModeFlags.FREE;
        }
        else if (FlagHelper.IsSet(prefab.GetComponent<BuildItem>().buildModeFlags, BuildModeFlags.POINTBASED))
        {
            buildMode = new PointBuild();
            buildModeFlag = BuildModeFlags.POINTBASED;
        }

        prepareBuildMode();
    }

    private void toggleBuildMode()
    {
        if (!canToggleBuildMode()) return;

        BuildMode oldMode = buildMode;

        string announceText = string.Empty;
        if (buildModeFlag == BuildModeFlags.FREE)
        {
            buildMode = new PointBuild();
            buildModeFlag = BuildModeFlags.POINTBASED;
            announceText = "POINTBASED";
        } else if (buildModeFlag == BuildModeFlags.POINTBASED)
        {
            buildMode = new FreeBuild();
            buildModeFlag = BuildModeFlags.FREE;
            announceText = "FREE";
        }

        UIHandler.Instance.announceEvent("BUILDMODE: " + announceText);

        oldMode.resetSpawnedObject();
        buildMode.setSpawnedObj(oldMode.getSpawnedObj());
        buildMode.prepare();
        prepareBuildMode();
    }

    private void prepareBuildMode()
    {
        buildMode.activeInvalidMaterial = activeInvalidMaterial;
        buildMode.activeValidMaterial = activeValidMaterial;
        buildMode.placementDistance = placementDistance;
        buildMode.headCamera = headCamera;
        buildMode.rotationSpeed = rotationSpeed;
    }

    private bool canToggleBuildMode()
    {
        BuildItem buildItem = prefabObj.GetComponent<BuildItem>();

        if (FlagHelper.IsSet(buildItem.supportedBuildModeFlags, BuildModeFlags.FREE))
            if (FlagHelper.IsSet(buildItem.supportedBuildModeFlags, BuildModeFlags.POINTBASED))
                return true;

        return false;
    }
}
