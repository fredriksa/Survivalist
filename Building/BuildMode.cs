using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode : MonoBehaviour {

    public int originalLayer;
    public Material originalMaterial;
    public Material activeValidMaterial;
    public Material activeInvalidMaterial;
    public Camera headCamera;

    public float placementDistance;
    public float rotationSpeed;

    protected GameObject spawnedObj;
    protected Renderer spawnedObjRenderer;

    protected bool canRotate, canRotateSideways, canAlignRotation;
    protected bool canPlace;

	public BuildMode()
    {

    }
	
	public virtual void update()
    {
        if (!canUpdate()) return;

        RaycastHit hit;
        Ray ray = CameraHelper.CenterRay(headCamera);

        //All masks but BuildingSnapPoint and Ignore Raycasts.
        int layerMask = ~(1 << LayerMask.NameToLayer("BuildingSnapPoint")) & ~(1 << LayerMask.NameToLayer("Ignore Raycast"));

        if (Physics.Raycast(ray, out hit, placementDistance, layerMask))
            OnActiveRayContact(hit);
        else
            OnActiveRayNoContact();

        if (canRotate)
            rotateObj();

        onPlace();
    }

    public virtual GameObject spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        spawnedObj = GameObject.Instantiate(prefab, position, rotation);
        return spawnedObj;
    }

    public virtual GameObject prepare()
    {
        Collider collider = spawnedObj.GetComponent<Collider>();
        if (collider)
            collider.isTrigger = true;

        originalLayer = spawnedObj.layer;
        spawnedObj.layer = LayerMask.NameToLayer("Ignore Raycast");

        Renderer renderer = spawnedObj.GetComponent<Renderer>();
        if (renderer)
        {
            originalMaterial = renderer.material;
            renderer.material = activeValidMaterial;
        }

        spawnedObjRenderer = spawnedObj.GetComponent<Renderer>();

        return spawnedObj;
    }

    public virtual void OnActiveRayContact(RaycastHit hit)
    {
        spawnedObj.transform.position = hit.point + new Vector3(0, ObjectHelper.getSizeFromRenderer(spawnedObj).y/2, 0);

        if (canAlignRotation && Input.GetKeyDown(KeyCode.Mouse1))
            alignRotation(hit);

        spawnedObjRenderer.material = activeValidMaterial;
        canPlace = true;
    }

    public virtual void destroyActiveObject()
    {
        if (spawnedObj)
            Destroy(spawnedObj);
    }

    public virtual bool isBuilding()
    {
        return !canUpdate();
    }


    protected void OnActiveRayNoContact()
    {
       spawnedObjRenderer.material = activeInvalidMaterial;

        Ray ray = CameraHelper.CenterRay(headCamera);
        spawnedObj.transform.position = ray.GetPoint(placementDistance);
        canPlace = false;
    }

    protected void rotateObj()
    {
        Vector3 direction = Vector3.zero;

        //Scrolls are "inverted", left results in Y axis rotation.
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (canRotateSideways && Input.GetKey(KeyCode.LeftControl))
                direction = Vector3.left;
            else if (canRotate)
                direction = Vector3.up;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (canRotateSideways && Input.GetKey(KeyCode.LeftControl))
                direction = Vector3.right;
            else if (canRotate)
                direction = Vector3.down;
        }

        if (direction != Vector3.zero)
            spawnedObj.transform.Rotate(direction * rotationSpeed, Space.Self);
    }

    protected void onPlace()
    {
        if (!canPlace || !Input.GetKey(KeyCode.Mouse0)) return;

        spawnedObj.GetComponent<Collider>().isTrigger = false;
        spawnedObj.layer = originalLayer;
        spawnedObj.GetComponent<Renderer>().material = originalMaterial;

        UIHandler.Instance.announceEvent("CONSTRUCTED " + spawnedObj.GetComponent<BuildItem>().itemName);

        modeReset();
    }

    protected virtual void modeReset()
    {
        spawnedObj = null;
    }

    protected virtual bool canUpdate()
    {
        if (headCamera == null || spawnedObj == null) return false;

        return true;
    }

    protected virtual void alignRotation(RaycastHit hit)
    {
        spawnedObj.transform.rotation = hit.transform.rotation;
    }
}
