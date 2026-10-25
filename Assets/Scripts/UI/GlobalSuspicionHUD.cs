using System;
using UnityEngine;
using UnityEngine.UI;

public class GlobalSuspicionHUD : MonoBehaviour
{
    [SerializeField] private RectTransform fillRect;
    [SerializeField] private Image fillImage;
    [ColorUsage(true, true)]
    [SerializeField] private Color glowColor = Color.white;

    private void Awake()
    {
        fillImage.color = glowColor;
    }

    private void Update()
    {
        float percent = GlobalSuspicionManager.Instance.GlobalSuspicionPercent;
        fillRect.localScale = new Vector3(1f - percent, 1f, 1f);
    }
}
