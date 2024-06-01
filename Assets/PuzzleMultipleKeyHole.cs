using System.Collections;
using System.Collections.Generic;
using tomi.SaveSystem;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleMultipleKeyHole : MonoBehaviour
{
    [SerializeField] private int activationsNeeded = 2;
    [SerializeField] private int currentActivations = 0;

    public UnityEvent onAllActivationsDone;

    public void OnActivation()
    {
        if (currentActivations < activationsNeeded)
        {
            currentActivations++;
        }
        if (currentActivations >= activationsNeeded)
        {
            currentActivations = activationsNeeded;
            onAllActivationsDone?.Invoke();

        }
    }

}
