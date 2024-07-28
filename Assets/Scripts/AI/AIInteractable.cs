using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIInteractable : MonoBehaviour, IAIInteractable
{
    [Header("References")]
    [SerializeField] private CheeseObject cheeseObject;
    [SerializeField] private bool respawnAfterAte = true;
    private Coroutine coroutine;

    [SerializeField]
    protected UnityEvent onInteracted;

    private void Awake()
    {
        if (cheeseObject == null)
            cheeseObject = GetComponent<CheeseObject>();
    }


    public void ConsoleLog()
    {
        Debug.Log("YAY AI Interacted with the Object: " + gameObject.name);
    }

    public void AIInteract()
    {
        //onInteracted?.Invoke();
    }

    public void AIInteract(float seconds)
    {
        cheeseObject.InteractableObject.CanInteract = false;
        cheeseObject.MeshR.enabled = false;
        cheeseObject.Collider.enabled = false;
        onInteracted?.Invoke();
        if (!respawnAfterAte) return;

        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(Despawn(seconds));
    }

    private IEnumerator Despawn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(Respawn(seconds));
    }

    private IEnumerator Respawn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        cheeseObject.Collider.enabled = true;
        cheeseObject.MeshR.enabled = true;
        cheeseObject.InteractableObject.CanInteract = true;
        cheeseObject.HoldableObject.Respawn();

    }
}
