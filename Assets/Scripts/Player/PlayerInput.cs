using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class PlayerInput : MonoBehaviour
{
    [ShowOnly][SerializeField] private PlayerReferences playerReferences;
    [SerializeField] private InputActionAsset playerControlsActions;
    [SerializeField] private MainControls playerControls;

    public InputActionAsset PlayerControls { get => playerControlsActions; set => playerControlsActions = value; }


    public Action TogglePause;


    private void OnValidate()
    {
        playerControls = new MainControls();

        GetPlayerRef();
    }

    private void Awake()
    {
        playerControls = new MainControls();

        GetPlayerRef();
    }

    private void OnEnable()
    {
        playerControls.Enable();

    }

    private void OnDisable()
    {
        playerControls.Disable();
    }


    private void GetPlayerRef()
    {
        if (playerReferences == null)
            playerReferences = GetComponent<PlayerReferences>();
    }

    public InputAction GetAction(string actionName)
    {
        return playerControlsActions.FindAction(actionName, true);
    }

    public void OnPause(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            TogglePause?.Invoke();
        }
    }
}
