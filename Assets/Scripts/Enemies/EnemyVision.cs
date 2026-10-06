using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private EnemyVisionConfigSO enemyVisionConfigSo;
    [SerializeField] private Light2D enemyLight;

    private void Awake()
    {
        enemyLight.pointLightOuterRadius = enemyVisionConfigSo.radius;
        enemyLight.pointLightOuterAngle = enemyVisionConfigSo.angle;
    }
}
