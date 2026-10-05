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
    [SerializeField] private float minDistance = 0.1f;

    private int _currentIndex;
    private float _waitTimer;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        
        if (_waitTimer > 0)
        {
            _waitTimer -= Time.fixedDeltaTime;

            if (_waitTimer <= 0)
            {
                _currentIndex = (_currentIndex + 1) % waypoints.Length;
            }
            
            return;
        }
        
        MoveToNextWaypoint();
    }

    private void MoveToNextWaypoint()
    {
        Vector2 nextWaypoint = waypoints[_currentIndex].point.position;
        
        Vector2 moveDirection = nextWaypoint - (Vector2)transform.position;
        float distance = Vector2.Distance(transform.position, nextWaypoint);
        
        _rb.linearVelocity = moveDirection.normalized * moveSpeed;

        float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        _rb.MoveRotation(targetAngle);
        
        if (distance <= minDistance)
        {
            float waitTime = waypoints[_currentIndex].waitTime;
            
            if (waitTime > 0)
            {
                _rb.linearVelocity = Vector2.zero;
                _waitTimer = waitTime;
            }
            else
            {
                _currentIndex = (_currentIndex + 1) % waypoints.Length;
            }
        }
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
