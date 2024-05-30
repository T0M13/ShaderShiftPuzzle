using UnityEngine;

public class FloatingRotator : MonoBehaviour
{
    public float hoverSpeed = 0.4f;
    public float hoverHeight = 0.2f;
    public float rotationSpeed = 50.0f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        Hover();
        Rotate();
    }

    void Hover()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    void Rotate()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnDrawGizmosSelected()
    {
        if (startPosition == Vector3.zero)
        {
            startPosition = transform.position; 
        }

        Gizmos.color = Color.cyan;
        Vector3 bottomPosition = new Vector3(startPosition.x, startPosition.y - hoverHeight, startPosition.z);
        Vector3 topPosition = new Vector3(startPosition.x, startPosition.y + hoverHeight, startPosition.z);
        Gizmos.DrawLine(bottomPosition, topPosition);

        Gizmos.DrawWireSphere(bottomPosition, 0.1f);
        Gizmos.DrawWireSphere(topPosition, 0.1f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * 0.5f); 
        Gizmos.DrawRay(transform.position, transform.right * 0.5f);
    }
}
