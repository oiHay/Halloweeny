using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Vector2 _moveInput;
    private Rigidbody2D _rb;
    private PlayerStateController _playerState;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerState = GetComponent<PlayerStateController>();
    }

    private void FixedUpdate()
    {
        if (!_playerState.CanAct)
        {
            _rb.linearVelocity = _moveInput * 0f;
            return;
        }
            
        _rb.linearVelocity = _moveInput * moveSpeed;
    }

    // Quando o player aperta algum input relacionado a ação de mover, o método guarda o valor referente ao input
    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }
}
