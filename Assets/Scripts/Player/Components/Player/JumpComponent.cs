using UnityEngine;
namespace tomi.CharacterController3D
{
    [CreateAssetMenu(fileName = "JumpBehaviour", menuName = "Behaviours/JumpBehaviour")]
    public class JumpComponent : ScriptableObject, JumpBehaviour
    {
        [Header("Jump Settings")]
        [SerializeField] float jumpForce;
        [SerializeField] bool isGrounded;
        [SerializeField] bool isJumping;
        [Header("Ground Check")]
        [SerializeField] float groundDetectionRange = 2;
        [SerializeField] LayerMask groundDetectionLayer;

        public float GroundDetectionRange { get => groundDetectionRange; set => groundDetectionRange = value; }
        public float JumpForce { get => jumpForce; set => jumpForce = value; }
        public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
        public bool IsJumping { get => isJumping; set => isJumping = value; }

        public void Jump(Rigidbody rb)
        {
            if (IsGrounded)
            {
                rb.AddForce(Vector2.up * JumpForce, ForceMode.Impulse);
                IsJumping = true;
            }
        }

        public bool IsGroundedCheck(Rigidbody rb)
        {
            if (Physics.Raycast(rb.transform.position, Vector2.down, GroundDetectionRange, groundDetectionLayer))
            {
                IsGrounded = true;
                IsJumping = false;
            }
            else
                IsGrounded = false;

            return IsGrounded;

        }
    }
}