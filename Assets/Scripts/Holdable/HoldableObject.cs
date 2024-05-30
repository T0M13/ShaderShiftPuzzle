using UnityEngine;

public class HoldableObject : MonoBehaviour, IHoldable
{
    public PlayerHoldTool playerHoldToolRef = null;
    public bool canBeHeld = true;
    [SerializeField] private Vector3 spawnPosition;

    private void Awake()
    {
        spawnPosition = transform.position;
    }

    public void Respawn()
    {
        transform.position = spawnPosition;

        if (GetComponent<AnimateCutoutPlus>())
        {
            GetComponent<AnimateCutoutPlus>().StartDissolve(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<DeathPlane>())
        {
            Respawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<DeathPlane>())
        {
            Respawn();
        }
    }
}
