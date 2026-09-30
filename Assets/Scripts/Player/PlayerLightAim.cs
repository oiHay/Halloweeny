using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLightAim : MonoBehaviour
{
    [SerializeField] private Transform coneLightTransform;

    private Vector2 _mouseScreenPos;

    private void OnLook(InputValue value)
    {
        _mouseScreenPos = value.Get<Vector2>();
    }

    private void LateUpdate()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(_mouseScreenPos.x, _mouseScreenPos.y - Camera.main.transform.position.z));
        
        Vector2 direction = (Vector2)mouseWorldPos - (Vector2)transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        coneLightTransform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}
