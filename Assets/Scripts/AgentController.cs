using UnityEngine;

public class AgentController : MonoBehaviour
{
    public string AgentName;

    [SerializeField]
    public float Speed;

    [SerializeField]
    float _maxHp;
    public float CurrentHp;
    Vector3 _targetPos;

    public delegate void NewTarget();
    public event NewTarget newTarget;

    void OnEnable()
    {
        newTarget += SetTargetPosition;
        CurrentHp = _maxHp;
        newTarget.Invoke();
        AgentName = RandomGenerator.NameGenerator("Agent");
        gameObject.name = AgentName;
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
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        UIController.CheckIfSelectedObjectDisabled(transform);
        newTarget -= SetTargetPosition;

        if (AgentSpawner.AgentsList is null) return;
        if (AgentSpawner.AgentsList.Contains(transform))
            AgentSpawner.AgentsList.Remove(transform);
    }
}
