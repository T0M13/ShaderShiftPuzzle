using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class TaskCheckDoTask : Node
{
    [Header("Agent Settings")]
    [SerializeField] private NavMeshAgent _agent;

    [Header("Animator Settings")]
    [SerializeField] private Animator _anim;
    [Header("Animations")]
    [SerializeField] private string IDLE_ANIMATION;
    [SerializeField] private string IDLE_EMOTION;
    [SerializeField] private float _idleSpeed = 8f;
    [SerializeField] private float _idleAgentChangeDirModifier = 3f;

    [SerializeField] private Transform _transform;
    [SerializeField] private float _FOVRange = 8f;
    [SerializeField] private LayerMask _signMask;

    [SerializeField] private float _noCheckTime = 5f;
    [SerializeField] private float _noCheckTimer;

    public TaskCheckDoTask(NavMeshAgent agent, Animator anim, Transform transform, float FOVrange, LayerMask signMask, float noCheckTime)
    {
        AnimalBT.pointToIdleTo = Vector3.zero;
        _agent = agent;
        _anim = anim;
        _transform = transform;
        _signMask = signMask;
        _noCheckTime = noCheckTime;
        _FOVRange = FOVrange;
    }

    public override NodeState Evaluate()
    {
        if (!MouseBT.checkForTask)
        {
            _noCheckTimer += Time.deltaTime;
            if (_noCheckTimer >= _noCheckTime)
            {
                _noCheckTimer = 0;
                MouseBT.checkForTask = true;
            }

            state = NodeState.FAILURE;
            return state;
        }

        object newPosition = GetData("targetNew");
        if (newPosition == null)
        {
            Collider[] colliders = Physics.OverlapSphere(
                _transform.position, _FOVRange, _signMask);

            if (colliders.Length > 0)
            {
                parent.parent.SetData("targetNew", colliders[0].transform);
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
