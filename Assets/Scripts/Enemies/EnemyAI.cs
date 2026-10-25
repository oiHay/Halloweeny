using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    #region Debug

    [SerializeField] private bool debugMode;

    private void DebugMessage(string message)
    {
        if(debugMode)
            Debug.Log(message);
    }

    #endregion
    
    public enum EnemyState { Patrulhando, Observando, Perseguindo, Procurando } // Dita os estados que o inimigo pode ter
    public enum SuspicionTier { None, Yellow, Orange, Red } // Determina os graus de suspeita que o inimigo pode ter
    
    public EnemyState CurrentState { get; private set; }
    public SuspicionTier CurrentTier { get; private set; }
    public EnemyVision Vision { get; private set; }

    public float Suspicion => _suspicion;
    public float SuspicionPercent => _suspicion / maxSuspicion;

    [Header("Thresholders")]
    [SerializeField] private float observeThreshold;
    [SerializeField] private float chaseThreshold;
    [SerializeField] private float redThreshold;
    
    [Header("GrowthRate")]
    [SerializeField] private float suspicionGrowthRate;
    [SerializeField] private float lowGrowthRate;
    [SerializeField] private float mediumGrowthRate;
    [SerializeField] private float highGrowthRate;
    
    [Header("Decay")]
    [SerializeField] private float decayRate;
    [SerializeField] private float minDecayRate = 0.3f;

    [Header("Others")]
    [SerializeField] private float arrivalDistance;
    [SerializeField] private float maxSuspicion;

    [Header("Rede de Segurança")] 
    [SerializeField] private float maxTravelTime = 3f;

    private EnemyDetection _detection;
    private EnemySimpleMovement _movement;
    private Vector2 _lastKnownPosition;
    private GameStateGate _gameState;
    private float _suspicion;
    private float _baseAngle;
    private float _travelTimer;

    private void Awake()
    {
        _detection = GetComponent<EnemyDetection>();
        _movement = GetComponent<EnemySimpleMovement>();
        _gameState = GetComponent<GameStateGate>();
        Vision = GetComponent<EnemyVision>();
    }

    private void FixedUpdate()
    {
        if (!_gameState.CanAct)
        {
            _movement.Stop();
            return;
        }

        DebugMessage("Tier: " + CurrentTier + " - State: " + CurrentState + " - Suspicion: " + _suspicion + " - Distance: " + Vector2.Distance(transform.position, _lastKnownPosition));
        
        switch (CurrentState) // Cada estado determina que tipo de ação deve ser determinada
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
        
        HandleSuspicion();
    }

    private void OnEnable() => EnemyRegistry.Instance.RegisterEnemy(this);
    private void OnDisable() => EnemyRegistry.Instance.UnregisterEnemy(this);

    private void GrowthSuspicion(float rate)
    {
        // Fórmula para determinar o grau de suspeita do inimigo, sendo rate qualquer variável dentro do header "GrowthRate"
        _suspicion = Mathf.Min(_suspicion + rate * Time.fixedDeltaTime, maxSuspicion); 
    }

    private void HandleSuspicion() // Método verifica o valor da variável "_suspicion" e determina qual o grau de suspeita do inimigo
    {
        if (_suspicion <= 0)                  CurrentTier = SuspicionTier.None;
        else if (_suspicion < chaseThreshold) CurrentTier = SuspicionTier.Yellow;
        else if (_suspicion < redThreshold)   CurrentTier = SuspicionTier.Orange;
        else                                  CurrentTier = SuspicionTier.Red;
    }

    private void HandlePatrulhando()
    {
        if (!_detection.IsPlayerDetected) return;
        GrowthSuspicion(suspicionGrowthRate);
            
        if (_suspicion >= observeThreshold) CurrentState = EnemyState.Observando;
    }

    private void HandleObservando()
    {
        _movement.Stop();

        if (_detection.IsPlayerDetected)
        {
            GrowthSuspicion(lowGrowthRate);
            if (_suspicion >= chaseThreshold) CurrentState = EnemyState.Perseguindo;
        }
        else
        {
            StartSearching();
        }
    }

    private void HandlePerseguindo()
    {
        GrowthSuspicion(_suspicion < redThreshold ? mediumGrowthRate : highGrowthRate);
        
        _movement.MoveTowards(_detection.PlayerPosition);

        if (_detection.IsPlayerDetected) return;
        StartSearching();
    }

    private void HandleProcurando()
    {
        float distance = Vector2.Distance(transform.position, _lastKnownPosition);
        bool hasArrived = distance < arrivalDistance || _travelTimer >= maxTravelTime;

        if (!hasArrived)
        {
            _travelTimer += Time.fixedDeltaTime;
            _baseAngle = _movement.MoveTowards(_lastKnownPosition);
        }
        else
        {
            _movement.Stop();
            _movement.LookAround(Time.time, _baseAngle);

            float decayAmount = Mathf.Max(decayRate * (1 - _suspicion / maxSuspicion), minDecayRate) * Time.fixedDeltaTime;
            _suspicion -= decayAmount;
        }

        if (_detection.IsPlayerDetected)
        {
            CurrentState = _suspicion >= chaseThreshold ? EnemyState.Perseguindo : EnemyState.Observando;
        }

        if (!(_suspicion <= 0)) return;
        _suspicion = 0;
        CurrentState = EnemyState.Patrulhando;
    }

    private void StartSearching()
    {
        _lastKnownPosition = _detection.PlayerPosition;
        _travelTimer = 0f;
        CurrentState = EnemyState.Procurando;
    }
}

