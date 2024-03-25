using UnityEngine;

public class HoldableObject : MonoBehaviour, IHoldable
{
    [SerializeField] private Vector3 spawnPosition;

    private void Awake()
    {
        spawnPosition = transform.position;
    }

    public void Respawn()
    {
        transform.position = spawnPosition;
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
