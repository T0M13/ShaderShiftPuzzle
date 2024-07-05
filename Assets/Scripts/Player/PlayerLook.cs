using System.Collections;
using System.Collections.Generic;
using tomi.CharacterController3D;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("Look")]
    [SerializeField] private PlayerReferences playerReferences;
    [SerializeField] private LookComponent lookComponent;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector2 lookPos;
    public Vector2 LookPos { get => lookPos; set => lookPos = value; }

    private void Awake()
    {
        if (playerTransform == null)
            GetPlayer();

        LockLook();
    }

    private void OnValidate()
    {
        if (playerTransform == null)
            GetPlayer();
    }

    private void LateUpdate()
    {
        if (playerReferences.CurrentState != PlayerState.Playing) return;
        if (playerReferences.IsInteractingRotatingObject) return;
        Look();
    }

    private void GetPlayer()
    {
        playerTransform = GetComponentInParent<PlayerReferences>().transform;
        lookComponent = playerTransform.GetComponent<LookComponent>();
    }

    private void Look()
    {
        lookComponent.Look(this.transform, this.LookPos, this.playerTransform);
    }

    private void LockLook()
    {
        lookComponent.LockMouse();
    }

    private void UnlockLook()
    {
        lookComponent.UnlockMouse();
    }

    private void OnLook(InputValue value)
    {
        this.LookPos = value.Get<Vector2>();
    }
}
