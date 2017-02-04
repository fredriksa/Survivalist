using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BuildSnapping : MonoBehaviour
{
    public bool hasSnapped = false;
    Vector3 objectSize;

    void Start()
    {
        objectSize = ObjectHelper.getSizeFromRenderer(gameObject);
    }
}
