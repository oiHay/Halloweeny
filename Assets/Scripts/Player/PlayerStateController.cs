using System;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    #region Debug

    [SerializeField] private bool debugMode;

    private void DebugMessage(string message)
    {
        if(debugMode)
            Debug.Log(message);
    }

    #endregion
    
    [SerializeField] private GameStateEventSO gameStateEventSo;
    
    public bool CanAct { get; private set; }

    private void OnEnable() => gameStateEventSo.OnRaised += Handler;
    private void OnDisable() => gameStateEventSo.OnRaised -= Handler;

    private void Handler(GameState state)
    {
        CanAct = (state == GameState.Play);

        DebugMessage("Estado de jogo: " + state + ", CanAct: " + CanAct);
    }
}
