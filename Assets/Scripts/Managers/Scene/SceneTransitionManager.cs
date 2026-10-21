using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [SerializeField] private float exitAnimDuration;
    [SerializeField] private float entranceAnimDuration;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void TransitionTo(string sceneName)
    {
        StartCoroutine(TransitionCoroutine(sceneName));
    }

    private IEnumerator TransitionCoroutine(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        if (asyncOperation != null) asyncOperation.allowSceneActivation = false;

        yield return new WaitForSeconds(exitAnimDuration);

        while (asyncOperation.progress < 0.9f)
        {
            yield return null;
        }
        asyncOperation.allowSceneActivation = true;

        yield return new WaitForSeconds(entranceAnimDuration);
    }
}
