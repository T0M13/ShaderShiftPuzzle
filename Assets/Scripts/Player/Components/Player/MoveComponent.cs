using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace tomi.CharacterController3D
{
    [CreateAssetMenu(fileName = "MoveBehaviour", menuName = "Behaviours/MoveBehaviour")]
    public class MoveComponent : ScriptableObject, MoveBehaviour
    {
        [Header("Move Settings")]
        [SerializeField] float moveSpeed;
        [SerializeField] float runSpeed;
        [SerializeField] float walkStepDistance = 1.5f;
        [SerializeField] float runStepDistance = 0.75f;
        [SerializeField] string stepSoundName = "FootStepsAsphalt";
        [SerializeField] string stepSoundNameDefault = "FootStepsAsphalt";
        [SerializeField] string stepSoundNameSnow = "FootStepsSnow";
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckDistance = 0.1f;

        private Vector3 lastPosition;
        private float distanceMoved;
        private bool resetMoved;

        public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
        public float RunSpeed { get => runSpeed; set => runSpeed = value; }

        public void Move(Rigidbody rb, Vector2 movement, bool isSprinting, bool isMoving)
        {
            Vector3 velocity;

            if (isSprinting)
            {
                velocity = new Vector3(movement.x * RunSpeed, rb.velocity.y, movement.y * RunSpeed);
            }
            else
            {
                velocity = new Vector3(movement.x * MoveSpeed, rb.velocity.y, movement.y * MoveSpeed);
            }

            rb.velocity = rb.transform.TransformDirection(velocity);

            HandleStepSound(rb, velocity, isSprinting, movement, isMoving);
        }

        private void HandleStepSound(Rigidbody rb, Vector3 velocity, bool isSprinting, Vector2 movement, bool isMoving)
        {
            if (!isMoving && !resetMoved)
            {
                resetMoved = true;
                if (AudioManager.Instance)
                {
                    AudioManager.Instance.StopSound(rb.gameObject);
                }
                return;
            }

            float currentStepDistance = isSprinting ? runStepDistance : walkStepDistance;
            distanceMoved += Vector3.Distance(rb.position, lastPosition);
            lastPosition = rb.position;

            if (IsGrounded(rb) && velocity.magnitude > 0)
            {
                if (distanceMoved >= currentStepDistance)
                {
                    if (AudioManager.Instance)
                    {
                        AudioManager.Instance.PlaySound(stepSoundName, rb.gameObject);
                        resetMoved = false;
                    }
                    distanceMoved = 0;
                }
            }
            else
            {
                if (AudioManager.Instance)
                {
                    AudioManager.Instance.StopSound(rb.gameObject);
                }
            }
        }

        private bool IsGrounded(Rigidbody rb)
        {
            RaycastHit hit;
            return Physics.Raycast(rb.position, Vector3.down, out hit, groundCheckDistance, groundLayer);
        }

        public void SetFootstepSoundForLevel()
        {
            string levelName = SceneManager.GetActiveScene().name;
            switch (levelName)
            {
                case "Level2":
                    stepSoundName = stepSoundNameSnow;
                    break;
                default:
                    stepSoundName = stepSoundNameDefault;
                    break;
            }
        }
    }
}
