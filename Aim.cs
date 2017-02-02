using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.Characters.FirstPerson;

public class Aim : MonoBehaviour {

	public FirstPersonController controller;

	public Vector3 currentAccuracy;
	public Vector3 accuracyWalking;
	public Vector3 accuracyRunning;
    public Vector3 projectileStrayFactor;

	void Update () 
	{
		if (controller == null)
			return;

		float speed = controller.getSpeed();

		if (speed < controller.getInitialSpeed ())
			resetAccuracy ();
		else
			updateAccuracy();
	}

	public Vector3 getAccuracy() { return currentAccuracy; }
	public Vector3 getAccuracyWalking() { return accuracyWalking; }
	public Vector3 getAcurracyRunning() { return accuracyRunning; }
    public Vector3 getProjectileStrayFavor() { return projectileStrayFactor; }

	private void resetAccuracy()
	{
		currentAccuracy = accuracyWalking;
	}

	private void updateAccuracy()
	{
		currentAccuracy = controller.isWalking() ? accuracyWalking : accuracyRunning;
	}
}
