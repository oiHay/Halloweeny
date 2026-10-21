using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    private PanelRenderer _panelRenderer;
    
    private Button _startGameButton;
    private Button _settingsButton;
    private Button _creditsButton;
    private Button _quitButton;

    private void Awake()
    {
        _panelRenderer = GetComponent<PanelRenderer>();
    }

    private void OnEnable()
    {
        _panelRenderer.RegisterUIReloadCallback(OnUIReload);
    }

    private void OnDisable()
    {
        _panelRenderer.UnregisterUIReloadCallback(OnUIReload);

        ClearButtonEvents();
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root, int version)
    {
        ClearButtonEvents();

        _startGameButton = root.Q<Button>("StartGame");
        _settingsButton =  root.Q<Button>("Settings");
        _creditsButton =   root.Q<Button>("Credits");
        _quitButton =      root.Q<Button>("ExitGame");
        
        if (_startGameButton != null) _startGameButton.clicked += OnStartClicked;
        if (_settingsButton != null)  _settingsButton.clicked  += OnSettingsClicked;
        if (_creditsButton != null)   _creditsButton.clicked   += OnCreditsClicked;
        if (_quitButton != null)      _quitButton.clicked      += OnQuitClicked;
    }

    private void OnStartClicked()
    {
        Debug.Log("Start clicked");
        SceneTransitionManager.Instance.TransitionTo("SampleScene");
    }
    
    private void OnSettingsClicked()
    {
        Debug.Log("Settings clicked");
    }
    
    private void OnCreditsClicked()
    {
        Debug.Log("Credits clicked");
    }
    
    private void OnQuitClicked()
    {
        Debug.Log("Quit clicked");
    }

    private void ClearButtonEvents()
    {
        if (_startGameButton != null) _startGameButton.clicked -= OnStartClicked;
        if (_settingsButton != null)  _settingsButton.clicked  -= OnSettingsClicked;
        if (_creditsButton != null)   _creditsButton.clicked   -= OnCreditsClicked;
        if (_quitButton != null)      _quitButton.clicked      -= OnQuitClicked;
    }
    
}
