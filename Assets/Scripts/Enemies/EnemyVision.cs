using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private EnemyVisionConfigSO enemyVisionConfigSo;
    [SerializeField] private Light2D enemyLight;
    [SerializeField] private GameObject presenceParticlePrefab;

    private ParticleSystem[] _particles;
    
    private void Awake()
    {
        enemyLight.pointLightOuterRadius = enemyVisionConfigSo.radius;
        enemyLight.pointLightOuterAngle = enemyVisionConfigSo.angle;

        GameObject spawnParticles = Instantiate(presenceParticlePrefab, transform, false);

        _particles = spawnParticles.GetComponentsInChildren<ParticleSystem>();
    }

    public void SetVisibleToPlayer(bool isVisible)
    {
        enemyLight.enabled = isVisible;

        if (isVisible)
            foreach (var particle in _particles) particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        else
            foreach (var particle in _particles) particle.Play();
    }
}
