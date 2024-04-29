using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerReferences : MonoBehaviour
{
    [Header("References")]
    [ShowOnly][SerializeField] private InputManager inputManager;
    [ShowOnly][SerializeField] private PlayerMovement playerMovement;
    [ShowOnly][SerializeField] private PlayerLook playerLook;
    [ShowOnly][SerializeField] private PlayerStats playerStats;
    [ShowOnly][SerializeField] private PlayerToolSelector playerToolSelector;
    [ShowOnly][SerializeField] private PlayerColorTool playerColorTool;
    [ShowOnly][SerializeField] private PlayerHoldTool playerHoldTool;
    [ShowOnly][SerializeField] private PlayerInteractableTool playerInteractableTool;
    [ShowOnly][SerializeField] private PlayerInput playerInput;
    [ShowOnly][SerializeField] private Health playerHealth;
    [Header("References Plus")]
    [SerializeField] private Rigidbody playerRigidBody;
    [SerializeField] private CapsuleCollider playerCollider;
    [ShowOnly][SerializeField] private ToolState currentToolState = ToolState.HoldTool;
    [ShowOnly][SerializeField] private PlayerState currentState = PlayerState.Playing;

    public InputManager InputManager { get => inputManager; set => inputManager = value; }
    public PlayerMovement PlayerMovement { get => playerMovement; set => playerMovement = value; }
    public Rigidbody PlayerRigidBody { get => playerRigidBody; set => playerRigidBody = value; }
    public CapsuleCollider PlayerCollider { get => playerCollider; set => playerCollider = value; }
    public PlayerLook PlayerLook { get => playerLook; set => playerLook = value; }
    public PlayerStats PlayerStats { get => playerStats; set => playerStats = value; }
    public Health PlayerHealth { get => playerHealth; set => playerHealth = value; }
    public PlayerColorTool PlayerColorTool { get => playerColorTool; set => playerColorTool = value; }
    public PlayerToolSelector PlayerToolSelector { get => playerToolSelector; set => playerToolSelector = value; }
    public PlayerHoldTool PlayerHoldTool { get => playerHoldTool; set => playerHoldTool = value; }
    public PlayerInteractableTool PlayerInteractableTool { get => playerInteractableTool; set => playerInteractableTool = value; }
    public PlayerInput PlayerInput { get => playerInput; set => playerInput = value; }
    public ToolState CurrentToolState { get => currentToolState; set => currentToolState = value; }
    public PlayerState CurrentState { get => currentState; set => currentState = value; }

    private void Awake()
    {
        GetReferences();
    }

    private void OnValidate()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        if (InputManager == null)
            InputManager = InputManager.instance;

        if (PlayerInput == null)
            PlayerInput = GetComponent<PlayerInput>();

        if (PlayerMovement == null)
            PlayerMovement = GetComponent<PlayerMovement>();

        if (PlayerRigidBody == null)
            PlayerRigidBody = GetComponent<Rigidbody>();

        if (PlayerCollider == null)
            PlayerCollider = GetComponent<CapsuleCollider>();

        if (PlayerLook == null)
            PlayerLook = GetComponentInChildren<PlayerLook>();

        if (PlayerStats == null)
            PlayerStats = GetComponent<PlayerStats>();

        if (PlayerColorTool == null)
            PlayerColorTool = GetComponent<PlayerColorTool>();

        if (PlayerHoldTool == null)
            PlayerHoldTool = GetComponent<PlayerHoldTool>();

        if (PlayerInteractableTool == null)
            PlayerInteractableTool = GetComponent<PlayerInteractableTool>();

        if (PlayerToolSelector == null)
            PlayerToolSelector = GetComponentInChildren<PlayerToolSelector>();

        //if (PlayerPostProcessingHandler == null)
        //    PlayerPostProcessingHandler = GetComponentInChildren<PlayerPostProcessingHandler>();

        if (PlayerHealth == null)
            PlayerHealth = GetComponent<Health>();

    }


}

public enum ToolState
{
    HoldTool,
    ColorTool,
}

public enum PlayerState
{
    Playing,
    Freeze,
}
