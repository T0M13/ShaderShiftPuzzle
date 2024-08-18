using UnityEngine;
namespace tomi.CharacterController3D
{
    public interface MoveBehaviour
    {
        public void Move(Rigidbody rb, Vector2 movement, bool isSprinting, bool isMoving);
    }
}
