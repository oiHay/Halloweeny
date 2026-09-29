using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStateEventSO", menuName = "Scriptable Objects/Event/GameStateEventSO")]
public class GameStateEventSO : ScriptableObject
{
    public event Action<GameState> OnRaised;
    public GameState currentGameState;

    private void OnEnable()
    {
        // Toda vez que é recriado ele limpa a lista de scripts que ouviam esse, serve para evitar o erro MissingReferenceException
        OnRaised = null;
    }

    public void Raise(GameState state)
    {
        // Variável que guarda na memória qual o estado atual do jogo
        currentGameState = state;
        // Notifica todos os scripts inscritos que o estado do jogo mudou
        OnRaised?.Invoke(state);
    }
}
