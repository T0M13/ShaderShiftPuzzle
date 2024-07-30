using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingGate : MonoBehaviour
{
    [SerializeField] private Transform targetTransform; 
    [SerializeField] private float transitionSpeed = 1.0f; 
    [SerializeField] private bool changeOnStart = false; 

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Vector3 targetScale;

    private bool isChanging = false;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;

        if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
            targetRotation = targetTransform.rotation;
            targetScale = targetTransform.localScale;
        }

        if (changeOnStart)
        {
            StartChange();
        }
    }

    void Update()
    {
        if (isChanging)
        {
            ChangeTransform();
        }
    }

    public void StartChange()
    {
        if (targetTransform == null)
        {
            Debug.LogWarning("Target transform is not set.");
            return;
        }
        isChanging = true;
    }

    public void StopChange()
    {
        isChanging = false;
    }

    private void ChangeTransform()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * transitionSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * transitionSpeed);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * transitionSpeed);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f &&
            Quaternion.Angle(transform.rotation, targetRotation) < 0.1f &&
            Vector3.Distance(transform.localScale, targetScale) < 0.01f)
        {
            transform.position = targetPosition;
            transform.rotation = targetRotation;
            transform.localScale = targetScale;
            isChanging = false;
        }
    }

    public void ResetTransform()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        transform.localScale = initialScale;
    }
}
