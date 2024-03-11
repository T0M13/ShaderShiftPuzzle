using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerReferences : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerLook playerLook;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerToolSelector playerToolSelector;
    [SerializeField] private PlayerColorTool playerColorTool;
    [SerializeField] private PlayerHoldTool playerHoldTool;
    [SerializeField] private PlayerInteractableTool playerInteractableTool;
    [SerializeField] private PlayerPostProcessingHandler playerPostProcessingHandler;
    [SerializeField] private Health playerHealth;
    [Header("References Plus")]
    [SerializeField] private Rigidbody playerRigidBody;
    [SerializeField] private CapsuleCollider playerCollider;

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
    public PlayerPostProcessingHandler PlayerPostProcessingHandler { get => playerPostProcessingHandler; set => playerPostProcessingHandler = value; }

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

        if (PlayerPostProcessingHandler == null)
            PlayerPostProcessingHandler = GetComponentInChildren<PlayerPostProcessingHandler>();

        if (PlayerHealth == null)
            PlayerHealth = GetComponent<Health>();

    }


}
