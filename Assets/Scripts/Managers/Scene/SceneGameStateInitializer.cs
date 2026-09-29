using System;
using UnityEngine;

public class SceneGameStateInitializer : MonoBehaviour
{
    [SerializeField] private GameState stateOnLoad;

    private void Start()
    {
        GameManager.Instance.ChangeState(stateOnLoad);
    }
}
