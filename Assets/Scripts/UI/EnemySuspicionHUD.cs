using System;
using UnityEngine;

public class EnemySuspicionHUD : MonoBehaviour
{
    private EnemyAI _enemyAI;
    private Canvas _canvasHUD;

    private void Awake()
    {
        _enemyAI = GetComponent<EnemyAI>();
    }
}
