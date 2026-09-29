using System;
using UnityEngine;

public class GameManager : MonoBehaviour
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
    
    public static GameManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeState(GameState newState)
    {
        // Método que permite que o gameManager mude o valor do estado atual da cena
        gameStateEventSo.Raise(newState);
        
        // Debug para saber o estado atual do jogo
        DebugMessage("Estado atual do jogo: " + gameStateEventSo.currentGameState.ToString());
    }
}