using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AgentSpawner : MonoBehaviour
{
    [SerializeField]
    Transform _ground;

    [SerializeField]
    public int GroundSize;

    [Range(3,5)]
    [SerializeField]
    int _agentStartNumber;

    [Range(5,30)]
    [SerializeField]
    public int MaxAgentsNumber;

    [Range(2,6)]
    [SerializeField]
    float _spawnDelay;

    public static List<Transform> AgentsList = new();

    public static AgentSpawner Instance;

    private void Awake()
    {
        Instance = this;
        _ground.localScale = new Vector3(GroundSize, 1,GroundSize);
    }

    void Start()
    {
        for (int i = 0; i < _agentStartNumber; i++)
            SpawnAgent(RandomGenerator.RandomVector(-GroundSize/0.2f, GroundSize/0.2f));

        StartCoroutine(SpawnerUpdate());
    }

    IEnumerator SpawnerUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnDelay);
            if (MaxAgentsNumber > AgentsList.Count)
                SpawnAgent(RandomGenerator.RandomVector(-GroundSize/0.2f, GroundSize /0.2f));
        }
    }

    void SpawnAgent(Vector3 spawnPos)
    {
        var agent = AgentsPooler.Instance.SpawnFromPool(spawnPos, Quaternion.identity);
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

    public static string NameGenerator(string name)
    {
        string number = null;
        for (int i = 0; i < 3; i++)
        {
            var rnd = Random.Range(0, 9);
            number += rnd.ToString();
        }

        return $"{name}:{number}";
    }
}

