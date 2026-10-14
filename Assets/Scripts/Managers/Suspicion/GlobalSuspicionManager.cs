using System.Collections.Generic;
using UnityEngine;

public class GlobalSuspicionManager : MonoBehaviour
{
    #region Debug

    [SerializeField] private bool debugMode;

    private void DebugMessage(string message)
    {
        if(debugMode)
            Debug.Log(message);
    }

    #endregion
    
    public static GlobalSuspicionManager Instance { get; private set; }

    public float GlobalSuspicionPercent => _globalSuspicion / maxGlobalSuspicion;
    
    [SerializeField] private float globalGrowthMultiplier;
    [SerializeField] private float globalDecayRate;
    [SerializeField] private float maxGlobalSuspicion;
    [SerializeField] private float chaseWeightMultiplier = 2f;

    private float _globalSuspicion;
    private List<EnemyAI> _activeEnemies = new List<EnemyAI>();
    
    private void Awake()
    {
        transform.parent = null;
        
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterEnemy(EnemyAI enemy)
    {
        _activeEnemies.Add(enemy);
    }

    public void UnregisterEnemy(EnemyAI enemy)
    {
        _activeEnemies.Remove(enemy);
    }

    private void FixedUpdate()
    {
        float suspicionSum = 0f;
        bool anyEnemyActive = false;
        
        foreach (var enemy in _activeEnemies)
        {
            if (enemy.CurrentState != EnemyAI.EnemyState.Patrulhando) anyEnemyActive = true;
            
            if (enemy.CurrentState != EnemyAI.EnemyState.Observando) suspicionSum += enemy.Suspicion;
            
            if (enemy.CurrentState != EnemyAI.EnemyState.Perseguindo) suspicionSum += enemy.Suspicion * chaseWeightMultiplier;
        }

        if (!anyEnemyActive) _globalSuspicion -= globalDecayRate * Time.fixedDeltaTime;
        else if (suspicionSum > 0) _globalSuspicion += suspicionSum * globalGrowthMultiplier * Time.fixedDeltaTime;
        
        _globalSuspicion = Mathf.Clamp(_globalSuspicion, 0f, maxGlobalSuspicion);
        
        if (_globalSuspicion >= maxGlobalSuspicion) GameManager.Instance.ChangeState(GameState.GameOver);
        
        DebugMessage("Porcentagem de Suspeita: " + GlobalSuspicionPercent);
    }
}
