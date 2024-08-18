using System.Collections;
using System.Collections.Generic;
using tomi.CharacterController3D;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private PlayerReferences playerReferences;
    [Header("References Plus")]
    private InputManager inputManager;
    private Rigidbody playerRigidBody;
    private Animator playerAnimator;
    private CapsuleCollider playerCollider;

    [Header("Movement")]
    [SerializeField] private MoveComponent moveComponent;
    [SerializeField] private Vector2 movement;
    [SerializeField] private bool canMove = true;
    [ShowOnly][SerializeField] private bool isMoving;
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool isSprinting;
    [Header("Jump")]
    [SerializeField] private JumpComponent jumpComponent;
    [SerializeField] private float jump;
    [SerializeField] private bool canJump = true;
    [SerializeField] private float fallingThreshhold = -10f;
    [ShowOnly][SerializeField] private bool isFalling;
    [Header("Look")]
    [SerializeField] private LookComponent lookComponent;
    private PlayerLook playerLook;
    [SerializeField] private Vector2 look;
    [SerializeField] private bool canLook = true;
    [Header("NoClip")]
    [SerializeField] private bool noClipActive = false;


    [Header("Gizmos")]
    [SerializeField] private bool showJumpGizmos = false;

    public MoveComponent MoveComponent { get => moveComponent; set => moveComponent = value; }

    private void Awake()
    {
        GetReferences();
    }

    private void OnValidate()
    {
        GetReferences();
    }

    private void Start()
    {
        moveComponent.SetFootstepSoundForLevel();
    }

    private void GetReferences()
    {
        if (playerReferences == null)
            playerReferences = GetComponent<PlayerReferences>();

        if (inputManager == null)
            inputManager = playerReferences.InputManager;

        if (playerRigidBody == null)
            playerRigidBody = playerReferences.PlayerRigidBody;

        if (playerCollider == null)
            playerCollider = playerReferences.PlayerCollider;

        if (playerLook == null)
            playerLook = playerReferences.PlayerLook;
    }



    private void Update()
    {
        if (playerReferences.CurrentState != PlayerState.Playing) return;
        Look();
        IsGrounded();
        Fall();
    }

    private void FixedUpdate()
    {
        if (playerReferences.CurrentState != PlayerState.Playing) return;
        Move();
        Jump();

    }

    private void Move()
    {
        if (!canMove) return;
        MoveComponent.Move(playerRigidBody, this.movement, this.isSprinting, this.isMoving);

        if (this.movement != Vector2.zero)
        {
            isMoving = true;
        }
        else
            isMoving = false;
    }

    private void Jump()
    {
        if (!canJump) return;
        if (jump == 0 || jumpComponent.IsJumping) return;
        jumpComponent.Jump(playerRigidBody);
        jump = 0;
    }


    private void Fall()
    {
        if (jumpComponent.IsJumping) return;
        if (playerRigidBody.velocity.y < fallingThreshhold)
        {
            this.isFalling = true;
        }
        else
        {
            this.isFalling = false;
        }
    }

    private bool IsGrounded()
    {
        return jumpComponent.IsGroundedCheck(playerRigidBody);
    }

    private void Look()
    {
        if (!canLook) return;
        Vector2 normalizedLookPos = new Vector2(this.look.x * Time.deltaTime, this.look.y * Time.deltaTime);
        playerReferences.PlayerLook.LookPos = normalizedLookPos;
    }

    public void Freeze()
    {
        movement = Vector2.zero;
        MoveComponent.Move(playerRigidBody, this.movement, this.isSprinting, this.isMoving);
        look = Vector2.zero;
        playerReferences.PlayerLook.LookPos = this.look;
    }

    public void ToggleNoClip()
    {
        noClipActive = !noClipActive;
        playerCollider.enabled = !noClipActive;
        if (playerRigidBody != null)
        {
            playerRigidBody.useGravity = !noClipActive;
        }
    }

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        if (showJumpGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * jumpComponent.GroundDetectionRange);
        }

    }
    #endregion

    #region Inputs
    public void OnMove(InputAction.CallbackContext value)
    {
        if (playerReferences.CurrentState != PlayerState.Playing) return;
        this.movement = value.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (playerReferences.CurrentState != PlayerState.Playing) return;
        this.jump = value.ReadValue<float>();
    }
    public void OnLook(InputAction.CallbackContext value)
    {
        if (playerReferences.CurrentState != PlayerState.Playing) return;
        this.look = value.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext value)
    {
        if (playerReferences.CurrentState != PlayerState.Playing) return;
        if (value.ReadValue<float>() >= 1f)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
    }

    #endregion
}
