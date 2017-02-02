using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseItem : MonoBehaviour
{
    [SerializeField]
    private int id = -1;

    //Warning: should never be used
    public void setId(int newId)
    {
        id = newId;
    }

    public int getId()
    {
        return id;
    }
}