using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace tomi.CharacterController3D
{
    [CreateAssetMenu(fileName = "MoveBehaviour", menuName = "Behaviours/MoveBehaviour")]
    public class MoveComponent : ScriptableObject, MoveBehaviour
    {
        [Header("Move Settings")]
        [SerializeField] float moveSpeed;
        [SerializeField] float runSpeed;

        public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
        public float RunSpeed { get => runSpeed; set => runSpeed = value; }

        public void Move(Rigidbody rb, Vector2 movement, bool isSprinting)
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
        }
    }
}