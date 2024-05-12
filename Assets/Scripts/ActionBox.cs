using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionBox : MonoBehaviour
{
    public UnityEvent actionToCall;
    public bool canBeCalled = true;
    public bool callOnce = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!canBeCalled) return;
        if (other.GetComponent<PlayerReferences>())
        {
            actionToCall?.Invoke();
            if (callOnce)
            {
                canBeCalled = false;
            }
        }
    }
}
