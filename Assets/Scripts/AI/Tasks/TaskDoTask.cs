using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class TaskDoTask : Node
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

    [Header("Do Task Settings")]
    [SerializeField] private Transform _transform;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _taskDoneRange;


    public TaskDoTask(NavMeshAgent agent, Animator anim, Transform transform, float maxDistance, float taskDoneRange)
    {
        AnimalBT.pointToIdleTo = Vector3.zero;
        _transform = transform;
        _agent = agent;
        _anim = anim;
        _maxDistance = maxDistance;
        _taskDoneRange = taskDoneRange;

    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("targetNew");
        _agent.SetDestination(target.position);
        float dist = Vector3.Distance(_transform.position, target.position);

        if (dist <= _taskDoneRange)
        {
            ClearData("targetNew");
            state = NodeState.SUCCESS;
            return state;
        }

        if (dist > _maxDistance)
        {
            ClearData("targetNew");
        }

        state = NodeState.RUNNING;
        return state;
    }


}
