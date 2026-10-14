using System;
using UnityEngine;

public class GlobalSuspicionHUD : MonoBehaviour
{
    [SerializeField] private RectTransform fillRect;

    private void Update()
    {
        float percent = GlobalSuspicionManager.Instance.GlobalSuspicionPercent;
        fillRect.localScale = new Vector3(1f - percent, 1f, 1f);
    }
}
