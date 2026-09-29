using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionReference pauseAction;

    private void OnEnable()
    {
        pauseAction.action.Enable();
        pauseAction.action.performed += OnPausePerformed;
    }

    private void OnDisable()
    {
        pauseAction.action.Disable();
        pauseAction.action.performed -= OnPausePerformed;
    }

    public void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        GameManager.Instance.TogglePause();
    }
}
