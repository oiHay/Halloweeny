using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private EnemyVisionConfigSO configSo;
    [SerializeField] private LayerMask obstacleMask;
    
    public bool IsPlayerDetected { get; private set; }
    public Vector2 PlayerPosition => _player.transform.position;
    
    private GameObject _player;
    private GameStateGate _gameState;
    
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _gameState = GetComponent<GameStateGate>();
    }

    private void FixedUpdate()
    {
        if (!_gameState.CanAct) return;
        
        CheckDetection();
    }

    private void CheckDetection()
    {
        IsPlayerDetected = false;
        
        Vector2 vectorToPlayer = PlayerPosition - (Vector2)transform.position;
        float distance = vectorToPlayer.magnitude;

        if (distance > configSo.radius) return;

        Vector2 direction = vectorToPlayer.normalized;

        float angleToPlayer = Vector2.Angle(transform.right, direction);
        
        if (angleToPlayer > configSo.angle / 2f) return;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, obstacleMask);
        IsPlayerDetected = hit.collider == null;
    }

    private void OnDrawGizmos()
    {
        if (_player == null) return;

        Gizmos.color = IsPlayerDetected ? Color.red : Color.white;
        Gizmos.DrawLine(transform.position, PlayerPosition);
    }
}
