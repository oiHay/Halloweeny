using UnityEngine;

[System.Serializable]
public struct Waypoint
{
    public Transform point;
    public float waitTime;
}

public class EnemySimpleMovement : MonoBehaviour
{
    [SerializeField] private Waypoint[] waypoints;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float lookAroundAngle = 30f;
    [SerializeField] private float lookAroundSpeed = 2;

    private Rigidbody2D _rb;
    private EnemyAI _enemyAI;
    private GameStateGate _gameState;
    private int _currentIndex;
    private float _waitTimer;
    private float _baseAngle;
    private float _minDistance = 0.1f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemyAI = GetComponent<EnemyAI>();
        _gameState = GetComponent<GameStateGate>();
    }

    private void FixedUpdate()
    {
        if (!_gameState.CanAct)
        {
            Stop();
            return;
        }
        
        if (_enemyAI.CurrentState != EnemyAI.EnemyState.Patrulhando) return;
        if (waypoints == null || waypoints.Length == 0) return;
        
        if (_waitTimer > 0)
        {
            _waitTimer -= Time.fixedDeltaTime;

            if (_waitTimer <= 0)
            {
                _currentIndex = (_currentIndex + 1) % waypoints.Length;
            }

            LookAround(_waitTimer, _baseAngle);
            
            return;
        }
        
        MoveToNextWaypoint();
    }

    public void LookAround(float timer, float baseAngle)
    {
        float offset = Mathf.Sin(timer * lookAroundSpeed) * lookAroundAngle;
        _rb.MoveRotation(baseAngle + offset);
    }
    
    private void MoveToNextWaypoint()
    {
        Vector2 nextWaypoint = waypoints[_currentIndex].point.position;

        float targetAngle = MoveTowards(nextWaypoint);
        
        float distance = Vector2.Distance(transform.position, nextWaypoint);
        
        if (distance <= _minDistance)
        {
            float waitTime = waypoints[_currentIndex].waitTime;

            if (waitTime > 0)
            {
                Stop();
                _waitTimer = waitTime;
                _baseAngle = targetAngle;
            }
            else
            {
                _currentIndex = (_currentIndex + 1) % waypoints.Length;
            }
        }
    }

    public float MoveTowards(Vector2 target)
    {
        Vector2 direction = target - (Vector2)transform.position;

        _rb.linearVelocity = direction.normalized * moveSpeed;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _rb.MoveRotation(targetAngle);

        return targetAngle;
    }

    public float FaceTowards(Vector2 target)
    {
        Vector2 direction = target - (Vector2)transform.position;
        
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _rb.MoveRotation(targetAngle);

        return targetAngle;
    }

    public void Stop()
    {
        _rb.linearVelocity = Vector2.zero;
    }
    
    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.DrawSphere(waypoints[i].point.position, 0.1f);
            Vector3 nextPoint = waypoints[(i + 1) % waypoints.Length].point.position;
            Gizmos.DrawLine(waypoints[i].point.position, nextPoint);
        }
    }
}
