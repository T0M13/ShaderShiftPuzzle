using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
using UnityEngine.AI;

public class TaskIdle : Node
{
    [Header("Agent Settings")]
    [SerializeField] private NavMeshAgent _agent;

    [Header("Animator Settings")]
    [SerializeField] private Animator _anim;
    [Header("Animations")]
    [SerializeField] private string IDLE_ANIMATION;
    [SerializeField] private string IDLE_EMOTION;



    [Header("Idle Settings")]
    [SerializeField] private Transform _centerPoint;
    [SerializeField] private float _idleRange = 10f;
    [SerializeField] private float _idleSpeed = 8f;
    [SerializeField] private float _idleAgentChangeDirModifier = 3f;


    public TaskIdle(NavMeshAgent agent, Animator anim, Transform centerPoint, float idleRange, float idleSpeed, float idleAgentChangeDirModifier)
    {
        AnimalBT.pointToIdleTo = Vector3.zero;
        _agent = agent;
        _anim = anim;
        _centerPoint = centerPoint;
        _idleRange = idleRange;
        _idleSpeed = idleSpeed;
        _idleAgentChangeDirModifier = idleAgentChangeDirModifier;
    }

    public TaskIdle(NavMeshAgent agent, Animator anim, Transform centerPoint, float idleRange, float idleSpeed, float idleAgentChangeDirModifier, string idleAnim, string idleEmote)
    {
        AnimalBT.pointToIdleTo = Vector3.zero;
        _agent = agent;
        _anim = anim;
        _centerPoint = centerPoint;
        _idleRange = idleRange;
        _idleSpeed = idleSpeed;
        _idleAgentChangeDirModifier = idleAgentChangeDirModifier;
        IDLE_ANIMATION = idleAnim;
        IDLE_EMOTION = idleEmote;
    }

    public override NodeState Evaluate()
    {
        AnimalIdle();

        state = NodeState.RUNNING;
        return state;
    }


    private void AnimalIdle()
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance + _idleAgentChangeDirModifier)
        {

            if (RandomPoint(_centerPoint.position, _idleRange, out AnimalBT.pointToIdleTo))
            {
                _agent.speed = _idleSpeed;
                SetAnimation();
                _agent.SetDestination(AnimalBT.pointToIdleTo);
            }
        }
    }

    private void SetAnimation()
    {
        if (!string.IsNullOrEmpty(IDLE_ANIMATION))
            _anim.Play(IDLE_ANIMATION);
        if (!string.IsNullOrEmpty(IDLE_EMOTION))
            _anim.Play(IDLE_EMOTION);
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

}
