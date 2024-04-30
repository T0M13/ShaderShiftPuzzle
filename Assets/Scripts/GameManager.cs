using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameState currentState;
    [SerializeField] private PlayerReferences player;
    [SerializeField] private CanvasManager canvasManager;

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
            Debug.LogError("GameManager: No canvas object found!");
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
        canvasManager.OnToggleCanvas(value);
    }

    private void EnterMainMenu()
    {
        Debug.Log("Entering Main Menu.");
        //Time.timeScale = 1;
        SetPlayerState(PlayerState.Freeze);
        SetCursorState(true);
    }

    private void EnterGame()
    {
        Debug.Log("Entering Game.");
        //Time.timeScale = 1;
        SetPlayerState(PlayerState.Playing);
        SetCursorState(false);
        OnToggleCanvas(false);
    }

    private void EnterPaused()
    {
        Debug.Log("Game is Paused.");
        //Time.timeScale = 0;
        SetPlayerState(PlayerState.Freeze);
        SetCursorState(true);
        OnToggleCanvas(true);
    }

    private void EnterGameOver()
    {
        Debug.Log("Game Over.");
        //Time.timeScale = 0;
        SetPlayerState(PlayerState.Freeze);
        SetCursorState(true);
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
        if (currentState == GameState.Game)
            ChangeState(GameState.Paused);
        else if (currentState == GameState.Paused)
            ChangeState(GameState.Game);
    }
}
