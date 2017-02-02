using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInteract : MonoBehaviour {
    public Camera camera;
    public GameObject actionUI;
    public KeyCode interactKey = KeyCode.E;

    private GameObject playerCameraOwner;
    private CharacterController playerCharController;

    private void Start()
    {
        playerCameraOwner = ObjectHelper.getParentGameObject(camera.gameObject, "Player");
        playerCharController = ObjectHelper.getParentGameObject(camera.gameObject, "Player").GetComponent<CharacterController>();
    }

	void Update ()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 3) && hit.transform.GetComponent<Item>())
        {
            if (Input.GetKeyDown(interactKey))
                OnInteraction(hit);
            else
                WhileInteractionLook(hit);
        }
        else
            WhileNoInteraction(hit);
	}

    private void WhileInteractionLook(RaycastHit hit)
    {
        actionUI.SetActive(hit.transform.GetComponent<Item>().WhileInteractionLook(playerCameraOwner));
    }

    private void OnInteraction(RaycastHit hit)
    {
        actionUI.SetActive(hit.transform.GetComponent<Item>().OnInteraction(playerCameraOwner));
    }

    private void WhileNoInteraction(RaycastHit hit)
    {
        actionUI.SetActive(false);
    }
}
