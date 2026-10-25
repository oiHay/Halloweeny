using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private EnemyVisionConfigSO enemyVisionConfigSo;
    [SerializeField] private Light2D enemyLight;
    [SerializeField] private ParticleSystem presenceParticlePrefab;

    
    private void Awake()
    {
        enemyLight.pointLightOuterRadius = enemyVisionConfigSo.radius;
        enemyLight.pointLightOuterAngle = enemyVisionConfigSo.angle;
    }

    public void SetVisibleToPlayer(bool isVisible)
    {
        enemyLight.enabled = isVisible;

        if (isVisible)
            Destroy(presenceParticlePrefab.gameObject);
        else
            Instantiate(presenceParticlePrefab, transform.position, Quaternion.identity);
    }
}
