using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace tomi.CharacterController3D
{
    [CreateAssetMenu(fileName = "LookBehaviour", menuName = "Behaviours/LookBehaviour")]
    public class LookComponent : ScriptableObject, LookBehaviour
    {
        [SerializeField] float minViewDistance = 25f;
        [SerializeField] float sensitivity = 100f;
        [SerializeField] float xRotation = 0f;

        public void Look(Transform transform, Vector2 mouse, Transform playerTransform)
        {
            float mouseX = mouse.x * sensitivity * Time.deltaTime;
            float mouseY = mouse.y * sensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, minViewDistance);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerTransform.Rotate(Vector3.up * mouseX);
        }

        public void LockMouse()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        public void UnlockMouse()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}