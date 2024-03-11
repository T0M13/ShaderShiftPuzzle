using UnityEngine;

namespace tomi.CharacterController3D
{
    public interface LookBehaviour
    {
        public void Look(Transform transform, Vector2 mouse, Transform playerTransform);
        public void LockMouse();
        public void UnlockMouse();

    }
}
