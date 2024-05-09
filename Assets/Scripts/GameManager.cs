using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameState currentState;
    [SerializeField] private PlayerReferences player;
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private SaveManager saveManager;

    public GameState CurrentState
    {
        get => currentState;
        private set => currentState = value;
    }

    public PlayerReferences Player
    {
        get => player;
        private set => player = value;
    }

    public CanvasManager CanvasManager
    {
        get => canvasManager;
        private set => canvasManager = value;
    }
    public SaveManager SaveManager
    {
        get => saveManager;
        private set => saveManager = value;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGame()
    {
        Player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerReferences>();
        if (Player == null)
        {
            Debug.LogError("GameManager: No player object found!");
        }
        else
        {
            Player.PlayerInput.TogglePause -= OnPause;
            Player.PlayerInput.TogglePause += OnPause;
        }

        canvasManager = GameObject.FindGameObjectWithTag("Canvas")?.GetComponent<CanvasManager>();
        if (canvasManager == null)
        {
            Debug.LogWarning("GameManager: No canvas object found!");
        }
        else
        {
            canvasManager.SetButtonFunctions(this);
        }

        saveManager = GameObject.FindGameObjectWithTag("SaveManager")?.GetComponent<SaveManager>();
        if (saveManager == null)
        {
            Debug.LogError("GameManager: No saveManager object found!");
        }



    }

    private void OnDestroy()
    {
        if (Player != null && Player.PlayerInput != null)
        {
            Player.PlayerInput.TogglePause -= OnPause;
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                ChangeState(GameState.MainMenu);
                break;
            case "Level0":
                InitializeGame();
                ChangeState(GameState.Game);
                break;
            default:
                Debug.LogWarning("Loaded scene not explicitly handled: " + scene.name);
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        OnStateChanged(newState);
    }

    private void OnStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                EnterMainMenu();
                break;
            case GameState.Game:
                EnterGame();
                break;
            case GameState.Paused:
                EnterPaused();
                break;
            case GameState.GameOver:
                EnterGameOver();
                break;
        }
    }

    private void OnToggleCanvas(bool value)
    {
        if (canvasManager == null)
        {
            Debug.LogWarning("Canvas Missing");
            return;
        }
        canvasManager.OnToggleCanvas(value);
    }

    private void EnterMainMenu()
    {
        Debug.Log("Entering Main Menu.");
        SetPlayerState(PlayerState.Freeze);
        SetCursorState(true);
    }

    private void EnterGame()
    {
        Debug.Log("Entering Game.");
        SetPlayerState(PlayerState.Playing);
        SetCursorState(false);
        OnToggleCanvas(false);
    }

    private void EnterPaused()
    {
        Debug.Log("Game is Paused.");
        SetPlayerState(PlayerState.Freeze);
        SetCursorState(true);
        OnToggleCanvas(true);
    }

    private void EnterGameOver()
    {
        //Save before?
        Debug.Log("Game Over.");
        SetPlayerState(PlayerState.Freeze);
        SetCursorState(true);
    }

    public void RestartLevel()
    {
        //Load Level/Save File?
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void BackToMainMenu()
    {
        //Save before?
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        //Save before?
        Application.Quit();
    }

    private void SetCursorState(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void SetPlayerState(PlayerState state)
    {

        if (player != null)
        {
            player.FreezePlayer();
            player.CurrentState = state;
        }
    }

    public void OnPause()
    {
        if (canvasManager.mainPanelManager.currentPanelIndex != 0) return;
        if (currentState == GameState.Game)
            ChangeState(GameState.Paused);
        else if (currentState == GameState.Paused)
            ChangeState(GameState.Game);
    }

    //private IEnumerator HandleCooldown()
    //{
    //    isCooldown = true; // Set cooldown flag to true
    //    yield return new WaitForSecondsRealtime(pauseCooldown); // Wait for cooldown period in real time
    //    isCooldown = false; // Reset cooldown flag after waiting
    //}
}
