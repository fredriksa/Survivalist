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

    public Material activeMaterial;

    private GameObject spawnedObj;
    private Material originalMaterial;

	void Start ()
    {
        if (!headCamera)
            Debug.LogWarning("BuildingSystem: Camera is not set for " + gameObject.name);

        if (!activeMaterial)
            Debug.LogWarning("BuildingSystem: Active material is not set for " + gameObject.name);
    }
	
	void Update ()
    {
        if (headCamera == null || spawnedObj == null) return;

        RaycastHit hit;
        Ray ray = CameraRay();

        if (Physics.Raycast(ray, out hit, placementDistance))
        {
            OnGroundContact(hit);
        }

        ModifyHeightObj();
        //Need to check if we object we are placing actually is colliding
        OnPlace();
    }

    public void build(GameObject obj)
    {
        if (spawnedObj) return;

        spawnedObj = GameObject.Instantiate(obj, transform.position, transform.rotation);
        spawnedObj.GetComponent<Collider>().enabled = false;
        originalMaterial = spawnedObj.GetComponent<Renderer>().material;
        spawnedObj.GetComponent<Renderer>().material = activeMaterial;
    }

    private void OnGroundContact(RaycastHit hit)
    {
        if (!Input.GetKey(heightModifierKey))
            spawnedObj.transform.position = hit.point;

        RotateObj();

        if (Input.GetKeyDown(alignRotationKey))
            AlignRotation(hit);
    }

    private void OnPlace()
    {
        if (!Input.GetKeyDown(placementKey)) return;

        spawnedObj.GetComponent<Collider>().enabled = true;
        spawnedObj.GetComponent<Renderer>().material = originalMaterial;
        systemReset();
    }

    private void systemReset()
    {
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
        {
            point = hit.point;
        } else
        {
            point = ray.GetPoint(placementDistance);
        }

        Vector3 spawnedObjPos = spawnedObj.transform.position;
        spawnedObj.transform.position = new Vector3(spawnedObjPos.x, point.y, spawnedObjPos.z);
    }

    private Ray CameraRay()
    {
        return headCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, headCamera.nearClipPlane));
    }
}
