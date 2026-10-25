using UnityEngine;

public class PlayerVisionDetection : MonoBehaviour
{
    public bool IsEnemyDetected { get; private set; }
    
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float radius;
    [SerializeField] private float angle;

    private void FixedUpdate()
    {
        foreach (var enemy in EnemyRegistry.Instance.Enemies)
        {
            bool isVisible = CheckVisibility(enemy);
            enemy.Vision.SetVisibleToPlayer(isVisible);
            Debug.Log(enemy.name + " visível: " + isVisible);
        }
    }

    private bool CheckVisibility(EnemyAI enemy)
    {
        Vector2 vectorToEnemy = (Vector2)enemy.transform.position - (Vector2)transform.position;
        float distance = vectorToEnemy.magnitude;

        if (distance > radius) return false;

        Vector2 direction = vectorToEnemy.normalized;
        float angleToEnemy = Vector2.Angle(transform.up, direction);

        if (angleToEnemy > angle / 2f) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, obstacleMask);
        return hit.collider == null;
    }
}
