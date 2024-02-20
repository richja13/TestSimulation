using System.Collections.Generic;
using UnityEngine;

public class AgentsPooler : MonoBehaviour
{
    public GameObject Agent;
    int _poolSize = 30;

    public List<GameObject> _pooledObjects;

    public static AgentsPooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _poolSize = AgentSpawner.Instance.MaxAgentsNumber;

        _pooledObjects = new List<GameObject>();

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject obj = Instantiate(Agent);
            obj.SetActive(false);
            _pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
                return _pooledObjects[i];
        }

        GameObject newObj = Instantiate(Agent);
        newObj.SetActive(false);
        _pooledObjects.Add(newObj);

        return newObj;
    }

    public GameObject SpawnFromPool(Vector3 position, Quaternion rotation)
    {
        GameObject objectToSpawn = GetPooledObject();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        return objectToSpawn;
    }
}
