using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private Vector3 closedPositionOffset;
    [SerializeField] private Vector3 openedPositionOffset;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private bool open = false;

    private Vector3 closedPosition;
    private Vector3 openedPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        UpdatePositions();
    }

    private void UpdatePositions()
    {
        closedPosition = transform.position + closedPositionOffset;
        openedPosition = transform.position + openedPositionOffset;
    }

    private void OnValidate()
    {
        if (!open)
            UpdatePositions();
    }

    public void OpenGate()
    {
        targetPosition = openedPosition;
        StopAllCoroutines();
        StartCoroutine(MoveGate(targetPosition));
        open = true;
    }

    public void CloseGate()
    {
        targetPosition = closedPosition;
        StopAllCoroutines();
        StartCoroutine(MoveGate(targetPosition));
        open = false;
    }

    private IEnumerator MoveGate(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(closedPosition, 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(openedPosition, 0.1f);
    }
}
