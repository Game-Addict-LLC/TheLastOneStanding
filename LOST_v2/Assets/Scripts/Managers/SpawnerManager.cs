using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public List<ObjectList> list;

    // Start is called before the first frame update
    void Start()
    {
        foreach (ObjectList objList in list)
        {
            objList.objectList[Random.Range(0, objList.objectList.Count - 1)].GetComponent<Spawner>().Spawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class ObjectList
{
    public List<GameObject> objectList;
}
