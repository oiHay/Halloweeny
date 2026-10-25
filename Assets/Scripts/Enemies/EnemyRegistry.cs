using System.Collections.Generic;
using UnityEngine;

public class EnemyRegistry : MonoBehaviour
{
    public static EnemyRegistry Instance { get; private set; }

    private readonly List<EnemyAI> _activeEnemies = new List<EnemyAI>();
    public IReadOnlyList<EnemyAI> Enemies => _activeEnemies;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void RegisterEnemy(EnemyAI enemy)
    {
        _activeEnemies.Add(enemy);
    }

    public void UnregisterEnemy(EnemyAI enemy)
    {
        _activeEnemies.Remove(enemy);
    }
}
