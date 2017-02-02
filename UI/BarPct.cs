using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarPct : MonoBehaviour {

    public float value = 100;
    public float maxValue = 100;

	void Update ()
    {
        float cover = 0;

        cover = (value == 0 || maxValue == 0) ? 0 : value / maxValue;
        //float cover = (float)(value / maxValue);

        Vector3 newLocalScale = new Vector3(cover, 1, 1);
        if (transform.localScale != newLocalScale)
        {
            transform.localScale = newLocalScale;
            transform.position = transform.position;
        }
	}
}
