using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHelper {

    public static Ray CenterRay(Camera camera)
    {
        return camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, camera.nearClipPlane));
    }
}
