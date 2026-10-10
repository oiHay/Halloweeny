using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Vector2 _moveInput;
    private Rigidbody2D _rb;
    private GameStateGate _gameState;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _gameState = GetComponent<GameStateGate>();
    }

    private void FixedUpdate()
    {
        if (!_gameState.CanAct)
        {
            _rb.linearVelocity = Vector2.zero;
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
