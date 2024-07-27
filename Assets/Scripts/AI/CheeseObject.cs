using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InteractableObject _interactableObject;
    [SerializeField] private HoldableObject _holdableObject;
    [SerializeField] private MeshRenderer _meshR;
    [SerializeField] private Collider _collider;


    public InteractableObject InteractableObject { get => _interactableObject; set => _interactableObject = value; }
    public HoldableObject HoldableObject { get => _holdableObject; set => _holdableObject = value; }
    public MeshRenderer MeshR { get => _meshR; set => _meshR = value; }
    public Collider Collider { get => _collider; set => _collider = value; }

    private void Awake()
    {
        if (InteractableObject == null)
            InteractableObject = GetComponent<InteractableObject>();
        if (HoldableObject == null)
            HoldableObject = GetComponent<HoldableObject>();
        if (MeshR == null)
            MeshR = GetComponent<MeshRenderer>();
        if (Collider == null)
            Collider = GetComponent<Collider>();
    }



}
