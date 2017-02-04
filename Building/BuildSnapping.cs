using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BuildSnapping : MonoBehaviour
{
    Vector3 objectSize;

    void Start()
    {
        objectSize = transform.GetComponent<Renderer>().bounds.size;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "BuildingSnapPoint") return;

        transform.gameObject.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
    }
}
