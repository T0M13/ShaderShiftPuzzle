using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionBoxObjects : MonoBehaviour
{
    public GameObject target;
    public UnityEvent actionToCall;
    public bool canBeCalled = true;
    public bool callOnce = true;

    private void OnTriggerEnter(Collider other)
    {
        if(target.GetComponent<Collider>() != other) return;
        if (!canBeCalled) return;
        actionToCall?.Invoke();
        if (callOnce)
        {
            canBeCalled = false;
        }
    }
}
