using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseItem : MonoBehaviour
{
    [SerializeField]
    private int id = -1;

    public void Update()
    {
    }

    //Warning: should only be used by Database
    public void setId(int newId)
    {
        id = newId;
    }

    public int getId()
    {
        return id;
    }
}