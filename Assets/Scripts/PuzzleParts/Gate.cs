using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] protected Vector3 closedPositionOffset;
    [SerializeField] protected Vector3 openedPositionOffset;
    [SerializeField] protected float moveSpeed = 0.3f;
    protected Vector3 closedPosition;
    protected Vector3 openedPosition;
    protected Vector3 targetPosition;
    protected bool open = false;

   
    protected virtual void Start()
    {
        UpdatePositions();
    }

    protected void UpdatePositions()
    {
        closedPosition = transform.position + closedPositionOffset;
        openedPosition = transform.position + openedPositionOffset;
    }

    protected virtual void OnValidate()
    {
        if (!open)
            UpdatePositions();
    }

    public virtual void OpenGate()
    {
        targetPosition = openedPosition;
        StopAllCoroutines();
        StartCoroutine(PlayGateSound());
        StartCoroutine(MoveGate(targetPosition));
        open = true;
    }

    public virtual void CloseGate()
    {
        targetPosition = closedPosition;
        StopAllCoroutines();
        StartCoroutine(PlayGateSound());
        StartCoroutine(MoveGate(targetPosition));
        open = false;
    }

    protected IEnumerator PlayGateSound()
    {
        yield return null;
        //AudioManager.Instance.PlaySound("MetalGate", gameObject);
        GetComponent<SoundPlayerOnObject>().PlaySelectedSound();
    }

    protected IEnumerator MoveGate(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(closedPosition, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(openedPosition, 0.1f);
    }
}
