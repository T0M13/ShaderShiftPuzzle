using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseTriggerBox : MonoBehaviour
{
    [SerializeField]
    protected UnityEvent onInteracted;
    public bool canBeCalled = true;
    public bool callOnce = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!canBeCalled) return;
        if (other.GetComponent<MouseBT>())
        {
            onInteracted.Invoke();
            if (callOnce)
            {
                canBeCalled = false;
            }
        }
    }
}
