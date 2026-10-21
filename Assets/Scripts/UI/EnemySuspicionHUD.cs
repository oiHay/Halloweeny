using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemySuspicionHUD : MonoBehaviour
{ 
    [SerializeField] private Image fillImage;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1f, 0f);
    
    [Header("Cor")]
    [SerializeField] private Color lowColor = Color.yellow;
    [SerializeField] private Color midColor = new Color(1f, 0.5f, 0f);
    [SerializeField] private Color highColor = Color.red;
    
    private EnemyAI _enemyAI;
    private Canvas _canvasHUD;

    private void Awake()
    {
        _enemyAI = GetComponentInParent<EnemyAI>();
        _canvasHUD = GetComponent<Canvas>();
    }

    private void LateUpdate()
    {
        transform.position = _enemyAI.transform.position + offset;
        transform.rotation = Quaternion.identity;

        _canvasHUD.enabled = _enemyAI.Suspicion > 0;
        fillImage.fillAmount = _enemyAI.SuspicionPercent;

        fillImage.color = _enemyAI.CurrentTier switch
        {
            EnemyAI.SuspicionTier.Yellow => lowColor,
            EnemyAI.SuspicionTier.Orange => midColor,
            EnemyAI.SuspicionTier.Red => highColor,
            _ => fillImage.color
        };
    }
}
