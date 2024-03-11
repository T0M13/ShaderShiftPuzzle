using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskCheckCheeseInRange : Node
{
    [Header("Transform Settings")]
    [SerializeField] private Transform _transform;
    [SerializeField] private float _cheeseRange = 8f;
    [SerializeField] private LayerMask _cheeseLayerMask;


    public TaskCheckCheeseInRange(Transform transform, float range, LayerMask cheeseLayerMask)
    {
        _transform = transform;
        _cheeseRange = range;
        _cheeseLayerMask = cheeseLayerMask;
    }

    public override NodeState Evaluate()
    {
        object cheese = GetData("cheese");
        if (cheese == null)
        {
            Collider[] colliders = Physics.OverlapSphere(
                _transform.position, _cheeseRange, _cheeseLayerMask);

            if (colliders.Length > 0)
            {
                if (colliders[0].gameObject.GetComponent<IAIInteractable>() != null)
                    colliders[0].gameObject.GetComponent<IAIInteractable>().AIInteract();
                parent.parent.SetData("cheese", colliders[0].transform);
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }
}
