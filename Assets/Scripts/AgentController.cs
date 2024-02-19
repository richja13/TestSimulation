using System;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    [SerializeField]
    public float Speed;

    [SerializeField]
    float _maxHp;
    protected float CurrentHp;
    Vector3 _targetPos;

    public delegate void NewTarget();
    public event NewTarget newTarget;

    void Start()
    {
        newTarget += SetTargetPosition;
        CurrentHp = _maxHp;
        newTarget.Invoke();
    }

    void Update()
    {
        CheckPosition();
    }

    void CheckPosition()
    {
        if (Vector2.Distance(_targetPos, transform.position) < 0.4f)
            newTarget?.Invoke();
        else
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, Speed * Time.deltaTime);
    }

    void SetTargetPosition()
    {
        var max = AgentSpawner.Instance.GroundSize / 0.2f;
        var min = -max;
        _targetPos = RandomGenerator.RandomVector(min,max);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Agent"))
            CheckHealth();
    }

    void CheckHealth()
    {
        CurrentHp--;
        if (CurrentHp <= 0)
        {
            AgentSpawner.Instance.AgentsList.Remove(transform);
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        newTarget -= SetTargetPosition;
    }
}
