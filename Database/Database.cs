using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for storing the original content copies as a database
public class Database : MonoBehaviour {
    public string databaseName;
    public List<GameObject> content = new List<GameObject>();

    void Start()
    {
        for (int id = 0; id < content.Count; id++)
        {
            GameObject obj = content[id];

            DatabaseItem dbItem = obj.GetComponent<DatabaseItem>();
            if (dbItem != null)
                dbItem.setId(id);
        }
    }

    public GameObject fetch(int id)
    {
        if (id < content.Count)
        {
            GameObject obj = content[id];
            return obj;
        }

        //Returns default value of generic type, e.g null for nullable, 0 for integers etc.
        return null;
    }

    public int fetchId(GameObject gobject)
    {
        foreach (GameObject obj in content)
        {
            //if ()
            //{
            //    return obj.GetComponent<DatabaseItem>().getId();
            //}
        }

        return -1;
    }
}


