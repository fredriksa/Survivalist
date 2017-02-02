using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {

	public Aim aimScript;

	public Vector3 initialScale;

	void Start ()
    {
		initialScale.x = transform.localScale.x;
		initialScale.y = transform.localScale.y;
		initialScale.z = transform.localScale.z;
	}

	void Update ()
    {
		if (!aimScript)
			return;

		if (aimScript.getAccuracy () == aimScript.getAccuracyWalking ())
			resetCrosshair ();
		else
			updateCrosshair ();
	}

	private void updateCrosshair()
	{
		Vector3 modifier = aimScript.getAccuracy ();
		transform.localScale =  new Vector3(initialScale.x * modifier.x, initialScale.y * modifier.y, initialScale.z * modifier.z);
	}
	
	private void resetCrosshair()
	{
		transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
	}
}
