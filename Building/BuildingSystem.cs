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

    private GameObject spawnedObj;
    private Material originalMaterial;
    private int originalLayer;

	void Start ()
    {
        if (!headCamera)
            Debug.LogWarning("BuildingSystem: Camera is not set for " + gameObject.name);

        if (!activeValidMaterial)
            Debug.LogWarning("BuildingSystem: Active material is not set for " + gameObject.name);

        Color color = activeValidMaterial.color;
        color.a = 0.6f;
        activeValidMaterial.color = color;

        //Color colorInactive = activeInvalidMaterial.color;
        //colorInactive.a = 0.6f;
        //activeInvalidMaterial.color = colorInactive;
    }
	
	void Update ()
    {
        if (headCamera == null || spawnedObj == null) return;

        BuildModeFlags buildMode = spawnedObj.GetComponent<BuildItem>().buildModeFlags;
        if (FlagHelper.IsSet(buildMode, BuildModeFlags.FREE))
            freeBuild();
        else
            pointBuild();
    }

    public void freeBuild()
    {
        RaycastHit hit;
        Ray ray = CameraRay();

        if (Physics.Raycast(ray, out hit, placementDistance))
            OnGroundContact(hit);

        RotateObj();
        ModifyHeightObj();
        OnPlace();
    }

    public void pointBuild()
    {
        RotateObj();

        RaycastHit hit;
        Ray ray = CameraRay();

        Vector3 point;
        if (Physics.Raycast(ray, out hit, placementDistance))
            point = hit.point;
        else
            point = ray.GetPoint(placementDistance);

        bool hitSnapPoint = false;
        if (Physics.Raycast(ray, out hit, placementDistance, LayerMask.NameToLayer("BuildingSnapPoint"))) hitSnapPoint = true;

        Vector3 newPos = Vector3.zero;
        if (hitSnapPoint)
        {
            newPos = hit.point;
            spawnedObj.GetComponent<Renderer>().material = activeValidMaterial;
        }

        if (!hitSnapPoint)
        {
            spawnedObj.GetComponent<Renderer>().material = activeInvalidMaterial;

            if (Physics.Raycast(ray, out hit, placementDistance))
                newPos = hit.point;
            else
                newPos = ray.GetPoint(placementDistance);
        }

        if (!spawnedObj.GetComponent<BuildSnapping>().hasSnapped)
            spawnedObj.transform.position = newPos + new Vector3(0, ObjectHelper.getSizeFromRenderer(spawnedObj).y/2, 0);
        else
        {
            spawnedObj.GetComponent<Renderer>().material = activeValidMaterial;
            OnPlace();
        }
    }

    public void build(GameObject obj)
    {
        if (spawnedObj)
        {
            Destroy(spawnedObj);
            systemReset();
        }

        spawnedObj = GameObject.Instantiate(obj, transform.position, transform.rotation);
        spawnedObj.GetComponent<Collider>().isTrigger = true;
        originalLayer = spawnedObj.layer;
        spawnedObj.layer = LayerMask.NameToLayer("Ignore Raycast");
        originalMaterial = spawnedObj.GetComponent<Renderer>().material;
        spawnedObj.GetComponent<Renderer>().material = activeValidMaterial;

        if (FlagHelper.IsSet(spawnedObj.GetComponent<BuildItem>().buildModeFlags, BuildModeFlags.POINTBASED))
        {
            spawnedObj.GetComponent<Renderer>().material = activeInvalidMaterial;
            spawnedObj.AddComponent<BuildSnapping>();
        }
    }

    private void OnGroundContact(RaycastHit hit)
    {
        if (!Input.GetKey(heightModifierKey))
            spawnedObj.transform.position = hit.point + new Vector3(0, ObjectHelper.getSizeFromRenderer(spawnedObj).y, 0);

        if (Input.GetKeyDown(alignRotationKey))
            AlignRotation(hit);
    }

    private void OnPlace()
    {
        if (!Input.GetKeyDown(placementKey)) return;

        spawnedObj.GetComponent<Collider>().isTrigger = false;
        spawnedObj.layer = originalLayer;
        spawnedObj.GetComponent<Renderer>().material = originalMaterial;

        UIHandler.Instance.announceEvent("CONSTRUCTED " + spawnedObj.GetComponent<BuildItem>().itemName);

        systemReset();
    }

    private void systemReset()
    {
        if (FlagHelper.IsSet(spawnedObj.GetComponent<BuildItem>().buildModeFlags, BuildModeFlags.POINTBASED))
        {
            Destroy(spawnedObj.GetComponent<BuildSnapping>());
        }

        spawnedObj = null;
    }

    private void AlignRotation(RaycastHit hit)
    {
        spawnedObj.transform.rotation = hit.transform.rotation;
    }

    private void RotateObj()
    {
        Vector3 direction = Vector3.zero;

        //Scrolls are "inverted", left results in Y axis rotation.
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {   
            if (Input.GetKey(rotationModifierKey))
                direction = Vector3.left;
            else
                direction = Vector3.up;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Input.GetKey(rotationModifierKey))
                direction = Vector3.right;
            else
                direction = Vector3.down;
        }

        if (direction != Vector3.zero)
            spawnedObj.transform.Rotate(direction * rotationSpeed, Space.Self);
    }

    private void ModifyHeightObj()
    {
        if (!Input.GetKey(heightModifierKey)) return;

        Ray ray = CameraRay();
        RaycastHit hit;

        Vector3 point;
        if (Physics.Raycast(ray, out hit, placementDistance))
            point = hit.point;
        else
            point = ray.GetPoint(placementDistance);

        Vector3 spawnedObjPos = spawnedObj.transform.position;
        spawnedObj.transform.position = new Vector3(spawnedObjPos.x, point.y, spawnedObjPos.z);
    }

    private Ray CameraRay()
    {
        return headCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, headCamera.nearClipPlane));
    }
}
