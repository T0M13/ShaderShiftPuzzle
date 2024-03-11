using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
using UnityEngine.AI;

public class TaskRunToCheese : Node
{

    [Header("Transform Settings")]
    [SerializeField] private Transform _transform;

    [Header("Agent Settings")]
    [SerializeField] private NavMeshAgent _agent;

    [Header("Animator Settings")]
    [SerializeField] private Animator _anim;

    [Header("Cheese Settings")]
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _eatRange;
    [SerializeField] private float _eatTime;
    [SerializeField] private float _eatTimer;
    [SerializeField] private bool canEat = true;

    [Header("Animations")]
    [SerializeField] private string CHASECHEESE_ANIMATION;
    [SerializeField] private string CHASECHEESE_EMOTION;
    [SerializeField] private string EAT_ANIMATION;
    [SerializeField] private string EAT_EMOTION;



    public TaskRunToCheese(Transform transform, Animator anim, NavMeshAgent agent, float maxDistance, float eatRange, float eatTime, bool shouldEat)
    {
        _transform = transform;
        _agent = agent;
        _anim = anim;
        _maxDistance = maxDistance;
        _eatRange = eatRange;
        _eatTime = eatTime;
        canEat = shouldEat;
    }

    public TaskRunToCheese(Transform transform, Animator anim, NavMeshAgent agent, float maxDistance, float eatRange, float eatTime, bool shouldEat, string chaseCheeseAnim, string chaseCheeseEmote, string eatAnim, string eatEmote)
    {
        _transform = transform;
        _agent = agent;
        _anim = anim;
        _maxDistance = maxDistance;
        _eatRange = eatRange;
        _eatTime = eatTime;
        canEat = shouldEat;
        CHASECHEESE_ANIMATION = chaseCheeseAnim;
        CHASECHEESE_EMOTION = chaseCheeseEmote;
        EAT_ANIMATION = eatAnim;
        EAT_EMOTION = eatEmote;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("cheese");
        _agent.SetDestination(target.position);
        float dist = Vector3.Distance(_transform.position, target.position);

        if (dist <= _eatRange)
        {
            _eatTimer += Time.deltaTime;
            SetEatAnimation();

            if (_eatTimer >= _eatTime)
            {
                _eatTimer = 0;
                if (canEat)
                {
                    Transform temp = (Transform)GetData("cheese");
                    if (temp.gameObject.GetComponent<AIInteractable>())
                        temp.gameObject.GetComponent<AIInteractable>().AIInteract(_eatTime);
                    MouseBT.checkForEnemy = false;
                    ClearData("cheese");
                }
            }
        }
        else
        {
            _eatTimer = 0;
            SetChaseAnimation();

        }

        if (dist > _maxDistance)
        {
            ClearData("cheese");
        }

        state = NodeState.RUNNING;
        return state;
    }

    private void SetChaseAnimation()
    {
        if (!string.IsNullOrEmpty(CHASECHEESE_ANIMATION))
            _anim.Play(CHASECHEESE_ANIMATION);
        if (!string.IsNullOrEmpty(CHASECHEESE_EMOTION))
            _anim.Play(CHASECHEESE_EMOTION);
    }

    private void SetEatAnimation()
    {
        if (!string.IsNullOrEmpty(EAT_ANIMATION))
            _anim.Play(EAT_ANIMATION);
        if (!string.IsNullOrEmpty(EAT_EMOTION))
            _anim.Play(EAT_EMOTION);
    }
}



