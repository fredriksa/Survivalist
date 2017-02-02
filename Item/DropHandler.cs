using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DropHandler : MonoBehaviour
{
    public static float dropSpeedForward = 200;
    //Returning false from either one of the hooks will interrupt the dropping process
    static public bool OnDrop(ItemStack stack, GameObject player)
    {
        DropPhysicalObjects(stack, player);
        stack.resetComponent();

        return true;   
    }

    static public bool OnDropBegin(ItemStack stack, GameObject player)
    {
        return true;
    }

    static public bool OnDropEnd(ItemStack stack, GameObject player)
    {
        return true;
    }

    static private void DropPhysicalObjects(ItemStack stack, GameObject player)
    {
        Camera camera = player.GetComponentInChildren<Camera>();

        List<GameObject> spawnedObjects = new List<GameObject>();

        foreach (Item item in stack.items)
        {
            Vector3 spawnPos = camera.gameObject.transform.position;

            GameObject itemObj = GameObject.Instantiate(item.gameObject, spawnPos, Quaternion.identity);

            if (!itemObj.GetComponent<Rigidbody>())
            {
                //Add rigidbody
                Rigidbody body = itemObj.AddComponent<Rigidbody>();
                Physics.IgnoreCollision(body.GetComponent<Collider>(), player.GetComponent<CharacterController>());

                //Setup collider for gidibody support
                MeshCollider collider = itemObj.GetComponent<MeshCollider>();
                if (collider)
                    collider.convex = true;

                //Shoot object forward
                float randomModifier = 0.1f;
                body.AddForce(camera.transform.forward * (dropSpeedForward * Random.Range(1 - randomModifier / 2, 1 + randomModifier / 2)));
            }

            spawnedObjects.Add(itemObj);
        }

        //Ignore collision with same objects
        foreach (GameObject obj in spawnedObjects)
            foreach (GameObject objInner in spawnedObjects)
                if (obj != objInner)
                    Physics.IgnoreCollision(obj.GetComponent<Collider>(), objInner.GetComponent<Collider>());
    }
}
