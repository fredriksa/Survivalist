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
    private int originalLayer;

	void Start ()
    {
        if (!headCamera)
            Debug.LogWarning("BuildingSystem: Camera is not set for " + gameObject.name);

        if (!activeMaterial)
            Debug.LogWarning("BuildingSystem: Active material is not set for " + gameObject.name);

        Color color = activeMaterial.color;
        color.a = 0.6f;
        activeMaterial.color = color;
    }
	
	void Update ()
    {
        if (headCamera == null || spawnedObj == null) return;

        RaycastHit hit;
        Ray ray = CameraRay();

        if (Physics.Raycast(ray, out hit, placementDistance))
            OnGroundContact(hit);

        ModifyHeightObj();
        //Need to check if we object we are placing actually is colliding
        OnPlace();
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
        spawnedObj.GetComponent<Renderer>().material = activeMaterial;
        spawnedObj.AddComponent<BuildSnapping>();
    }

    private void OnGroundContact(RaycastHit hit)
    {
        if (!Input.GetKey(heightModifierKey))
            spawnedObj.transform.position = hit.point;// + new Vector3(0, GetSpawnedObjModifiers().y, 0);

        RotateObj();

        if (Input.GetKeyDown(alignRotationKey))
            AlignRotation(hit);
        /*
        //Transform the hit point to local point and nomralize it's direction
        Vector3 localPoint = hit.transform.InverseTransformPoint(hit.point);
        Vector3 localDir = localPoint.normalized;

        float upDot = Vector3.Dot(localDir, Vector3.up);
        float forwardDot = Vector3.Dot(localDir, Vector3.forward);
        float rightDot = Vector3.Dot(localDir, Vector3.right);

        float upPower = Mathf.Abs(upDot);
        float forwardPower = Mathf.Abs(forwardDot);
        float rightPower = Mathf.Abs(rightDot);

        Vector3 offset = Vector3.zero;

        //Right or left collision
        if (rightPower > forwardPower)
        {
            if (rightDot > 0)
            {
                offset.x = GetSpawnedObjModifiers().x;
                Debug.Log("Right");
            }
            else
            {
                offset.x = -GetSpawnedObjModifiers().x;
                Debug.Log("Left");
            }
        }


        //Front or back collision
        if (forwardPower > rightPower)
        { 
            //Forward
            if (forwardDot > 0)
                offset.z = GetSpawnedObjModifiers().z;
            else
                offset.z = -GetSpawnedObjModifiers().z;
        }

        offset.y = GetSpawnedObjModifiers().y; //Always spawn obj ontop of looking point
        //spawnedObj.transform.position += offset;*/
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
        Destroy(spawnedObj.GetComponent<BuildSnapping>());
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

    private Vector3 GetSpawnedObjModifiers()
    {
        Renderer renderer = spawnedObj.GetComponent<Renderer>();
        if (!renderer) return Vector3.zero;

        return new Vector3(renderer.bounds.size.x / 2, renderer.bounds.size.y / 2, renderer.bounds.size.z / 2);
    }
}
