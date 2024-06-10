using System.Collections;
using System.Collections.Generic;
using tomi.SaveSystem;
using UnityEngine;
namespace tomi.CharacterController3D
{
    [CreateAssetMenu(fileName = "LookBehaviour", menuName = "Behaviours/LookBehaviour")]
    public class LookComponent : ScriptableObject, LookBehaviour
    {
        [SerializeField] float minViewDistance = 25f;
        //[SerializeField] float sensitivity = 100f;
        [SerializeField][ShowOnly] float xRotation = 0f;
        [SerializeField] bool reverseMouse = false;

        public void Look(Transform transform, Vector2 mouse, Transform playerTransform)
        {
            float mouseX = mouse.x * SaveData.Current.playerProfile.aimSensitivity;
            float mouseY = (reverseMouse ? -mouse.y : mouse.y) * SaveData.Current.playerProfile.aimSensitivity;

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