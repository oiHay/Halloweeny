using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Patrulhando, Observando, Perseguindo, Procurando }
    
    public EnemyState CurrentState { get; private set; }

    [SerializeField] private float observeThreshold;
    [SerializeField] private float chaseThreshold;
    [SerializeField] private float suspicionGrowthRate;
    [SerializeField] private float searchDuration;
    [SerializeField] private float arrivalDistance;

    private EnemyDetection _detection;
    private EnemySimpleMovement _movement;
    private float _suspicion;
    private float _searchTimer;
    private float _baseAngle;
    private Vector2 _lastKnownPosition;
    private GameStateGate _gameState;
    

    private void Awake()
    {
        _detection = GetComponent<EnemyDetection>();
        _movement = GetComponent<EnemySimpleMovement>();
        _gameState = GetComponent<GameStateGate>();
    }

    private void FixedUpdate()
    {
        if (!_gameState.CanAct)
        {
            _movement.Stop();
            return;
        }
        
        switch (CurrentState)
        {
            case EnemyState.Patrulhando:
                HandlePatrulhando();
                break;
            
            case EnemyState.Observando:
                HandleObservando();
                break;
            
            case EnemyState.Perseguindo:
                HandlePerseguindo();
                break;
            
            case EnemyState.Procurando:
                HandleProcurando();
                break;
        }
    }

    private void HandlePatrulhando()
    {
        if (!_detection.IsPlayerDetected) return;
        _suspicion += suspicionGrowthRate * Time.fixedDeltaTime;
            
        if (_suspicion >= observeThreshold) CurrentState = EnemyState.Observando;
    }

    private void HandleObservando()
    {
        _movement.Stop();

        if (_detection.IsPlayerDetected)
        {
            _suspicion += suspicionGrowthRate * Time.fixedDeltaTime;
            if (_suspicion >= chaseThreshold) CurrentState = EnemyState.Perseguindo;
        }
        else
        {
            StartSearching();
        }
    }

    private void HandlePerseguindo()
    {
        _movement.MoveTowards(_detection.PlayerPosition);

        if (_detection.IsPlayerDetected) return;
        StartSearching();
    }

    private void HandleProcurando()
    {
        float distance = Vector2.Distance(transform.position, _lastKnownPosition);

        if (distance >= arrivalDistance)
        {
            _baseAngle = _movement.MoveTowards(_lastKnownPosition);
        }
        else
        {
            _movement.Stop();
            _movement.LookAround(_searchTimer, _baseAngle);

            _searchTimer -= Time.fixedDeltaTime;
        }

        if (_detection.IsPlayerDetected)
        {
            CurrentState = _suspicion >= chaseThreshold ? EnemyState.Perseguindo : EnemyState.Observando;
        }

        if (!(_searchTimer <= 0)) return;
        _suspicion = 0;
        CurrentState = EnemyState.Patrulhando;
    }

    private void StartSearching()
    {
        _lastKnownPosition = _detection.PlayerPosition;
        _searchTimer = searchDuration;
        CurrentState = EnemyState.Procurando;
    }
}

