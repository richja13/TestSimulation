using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AgentSpawner : MonoBehaviour
{
    [SerializeField]
    Transform _ground;

    [SerializeField]
    int _groundSize;

    [Range(3,5)]
    [SerializeField]
    int _agentStartNumber;

    [Range(5,30)]
    [SerializeField]
    int _maxAgentsNumber;

    [SerializeField]
    public GameObject Agent;

    [Range(2,6)]
    [SerializeField]
    float _spawnDelay;

    public List<Transform> AgentsList;

    void OnEnable()
    {
        _ground.localScale = new Vector3(_groundSize, 1,_groundSize);
    }

    void Start()
    {
        for (int i = 0; i < _agentStartNumber; i++)
            SpawnAgent(RandomGenerator.RandomVector(-_groundSize/0.2f, _groundSize/0.2f));

        StartCoroutine(SpawnerUpdate());
    }

    IEnumerator SpawnerUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnDelay);
            if (_maxAgentsNumber > AgentsList.Count)
                SpawnAgent(RandomGenerator.RandomVector(-_groundSize/0.2f, _groundSize/0.2f));
        }
    }

    void SpawnAgent(Vector3 spawnPos)
    {
        var agent = Instantiate(Agent, spawnPos, transform.rotation);
        AgentsList.Add(agent.transform);
    }
}

public class RandomGenerator
{
    public static Vector3 RandomVector(float min, float max)
    { 
        var x = Random.Range(min, max);
        var z = Random.Range(min, max);
        return new Vector3(x,1,z);
    }
}

