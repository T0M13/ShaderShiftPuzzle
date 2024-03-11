using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AIInteractable _aIInteractable;
    [SerializeField] private InteractableObject _interactableObject;
    [SerializeField] private HoldableObject _holdableObject;
    [SerializeField] private MeshRenderer _meshR;
    [SerializeField] private Collider _collider;
    [Header("References TEMP")]
    [SerializeField] private GameObject _potion;
    [SerializeField] private MouseBT _mouseAI;

    public AIInteractable AIInteractable { get => _aIInteractable; set => _aIInteractable = value; }
    public InteractableObject InteractableObject { get => _interactableObject; set => _interactableObject = value; }
    public HoldableObject HoldableObject { get => _holdableObject; set => _holdableObject = value; }
    public MeshRenderer MeshR { get => _meshR; set => _meshR = value; }
    public Collider Collider { get => _collider; set => _collider = value; }

    private void Awake()
    {
        if (AIInteractable == null)
            AIInteractable = GetComponent<AIInteractable>();
        if (InteractableObject == null)
            InteractableObject = GetComponent<InteractableObject>();
        if (HoldableObject == null)
            HoldableObject = GetComponent<HoldableObject>();
        if (MeshR == null)
            MeshR = GetComponent<MeshRenderer>();
        if (Collider == null)
            Collider = GetComponent<Collider>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _potion)
        {
            DoubleSize(other.gameObject);
        }
    }


    private void DoubleSize(GameObject potion)
    {
        potion.SetActive(false);
        transform.localScale = transform.localScale * 2f;
        _mouseAI.EatTime = _mouseAI.EatTime * 2f;
    }

}
