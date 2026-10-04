using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    private Vector2 _mouseScreenPos;

    // Quando o player mexe o mouse, o método calcula o valor de x e y e salva o valor
    private void OnLook(InputValue value)
    {
        _mouseScreenPos = value.Get<Vector2>();
    }

    private void LateUpdate()
    {
        // Usa os valores salvos no método OnLook e da câmera para determinar a posição do mouse na cena
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(_mouseScreenPos.x, _mouseScreenPos.y, -Camera.main.transform.position.z));
        
        // A posição menos a rotação do mouse determinam a direção do mesmo
        Vector2 direction = (Vector2)mouseWorldPos - (Vector2)transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}
