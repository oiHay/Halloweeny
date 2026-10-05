using System;
using UnityEngine;

public class EnemySimpleMovement : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float minDistance = 0.1f;

    private int _currentIndex;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (waypoints.Length == 0 || waypoints == null) return;
        
        MoveToNextWaypoint();
    }

    private void MoveToNextWaypoint()
    {
        Vector2 nextWaypoint = waypoints[_currentIndex].position;
        
        Vector2 moveDirection = nextWaypoint - (Vector2)transform.position;
        float distance = Vector2.Distance(transform.position, nextWaypoint);
        
        _rb.linearVelocity = moveDirection.normalized * moveSpeed;

        float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        _rb.MoveRotation(targetAngle);
        
        if (distance <= minDistance)
        {
            _currentIndex = (_currentIndex + 1) % waypoints.Length;
        }
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.DrawSphere(waypoints[i].position, 0.1f);
            Vector3 nextPoint = waypoints[(i + 1) % waypoints.Length].position;
            Gizmos.DrawLine(waypoints[i].position, nextPoint);
        }
    }
}
