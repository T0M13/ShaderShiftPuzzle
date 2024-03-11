using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
using UnityEngine.AI;

public class TaskRunAway : Node
{

    [Header("Transform Settings")]
    [SerializeField] private Transform _transform;

    [Header("Agent Settings")]
    [SerializeField] private NavMeshAgent _agent;

    [Header("Animator Settings")]
    [SerializeField] private Animator _anim;

    [Header("RunAway Settings")]
    [SerializeField] private float _checkRange = 10f;
    [SerializeField] private float _checkRangeAddModifier = 10f;
    [SerializeField] private float _runAwaySpeed = 18f;
    [SerializeField] private float _runAwayDisplacementDist = 18f;
    [SerializeField] private bool _hide = true;
    [SerializeField] private Transform[] _hidePoints;
    [SerializeField] private float _waitHidden = 3f;
    [Header("Hiding Settings")]
    [SerializeField] private bool hiding = false;
    [SerializeField] private bool randomHiding = false;
    [SerializeField] private float waitTimer = 0f;

    [Header("Animations")]
    [SerializeField] private string RUNAWAY_ANIMATION;
    [SerializeField] private string RUNAWAY_EMOTION;
    [SerializeField] private string HIDING_ANIMATION;
    [SerializeField] private string HIDING_EMOTION;

    [SerializeField] private GameObject _mouseCam;

    public TaskRunAway(Transform transform, Animator anim, NavMeshAgent agent, float checkRange, float checkRangeAddModifier, float runAwaySpeed, float runAwayDisplacementDist, bool hide, Transform[] hidePoints, float waitHidden)
    {
        AnimalBT.pointToRunAwayTo = Vector3.zero;
        _transform = transform;
        _agent = agent;
        _anim = anim;
        _checkRange = checkRange;
        _checkRangeAddModifier = checkRangeAddModifier;
        _runAwaySpeed = runAwaySpeed;
        _runAwayDisplacementDist = runAwayDisplacementDist;
        _hide = hide;
        _hidePoints = hidePoints;
        _waitHidden = waitHidden;
    }

    public TaskRunAway(Transform transform, Animator anim, NavMeshAgent agent, float checkRange, float checkRangeAddModifier, float runAwaySpeed, float runAwayDisplacementDist, bool hide, Transform[] hidePoints, float waitHidden,
       string runAwayAnim, string runAwayEmote, string hideAnim, string hideEmote, GameObject mouseCam)
    {
        AnimalBT.pointToRunAwayTo = Vector3.zero;
        _transform = transform;
        _agent = agent;
        _anim = anim;
        _checkRange = checkRange;
        _checkRangeAddModifier = checkRangeAddModifier;
        _runAwaySpeed = runAwaySpeed;
        _runAwayDisplacementDist = runAwayDisplacementDist;
        _hide = hide;
        _hidePoints = hidePoints;
        _waitHidden = waitHidden;

        RUNAWAY_ANIMATION = runAwayAnim;
        RUNAWAY_EMOTION = runAwayEmote;
        HIDING_ANIMATION = hideAnim;
        HIDING_EMOTION = hideEmote;

        _mouseCam = mouseCam;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        float dist = Vector3.Distance(_transform.position, target.position);

        if (_hide && _hidePoints.Length > 0)
        {
            if (!hiding)
            {
                _agent.speed = _runAwaySpeed;
                Vector3 newHidePoint;
                if (randomHiding)
                {
                    newHidePoint = _hidePoints[Random.Range(0, _hidePoints.Length)].position;
                }
                else
                {
                    newHidePoint = GetClosestHidePoint(_hidePoints).position;
                }
                AnimalBT.pointToRunAwayTo = newHidePoint;
                _agent.SetDestination(newHidePoint);
                SetRunAwayAnimation();
                hiding = true;
                _mouseCam.SetActive(true);
            }
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (hiding)
                {
                    waitTimer += Time.deltaTime;
                    SetHidingAnimation();
                    if (waitTimer > _waitHidden)
                    {
                        waitTimer = 0;
                        hiding = false;
                        ClearData("target");
                        _mouseCam.SetActive(false);
                    }
                }
            }
        }
        else
        {
            if (dist < _checkRange + _checkRangeAddModifier)
            {
                _agent.speed = _runAwaySpeed;
                Vector3 normDir = (target.position - _transform.position).normalized;
                AnimalBT.pointToRunAwayTo = (_transform.position - (normDir * _runAwayDisplacementDist));
                SetRunAwayAnimation();
                _agent.SetDestination(AnimalBT.pointToRunAwayTo);
            }
            else
            {
                ClearData("target");
            }
        }


        state = NodeState.RUNNING;
        return state;
    }

    Transform GetClosestHidePoint(Transform[] hidepoints)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = _transform.position;
        foreach (Transform t in hidepoints)
        {
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }

    private void SetHidingAnimation()
    {

        if (!string.IsNullOrEmpty(HIDING_ANIMATION))
            _anim.Play("Idle_A");
        if (!string.IsNullOrEmpty(HIDING_EMOTION))
            _anim.Play("Eyes_Trauma");

    }

    private void SetRunAwayAnimation()
    {
        if (!string.IsNullOrEmpty(RUNAWAY_ANIMATION))
            _anim.Play("Run");
        if (!string.IsNullOrEmpty(RUNAWAY_EMOTION))
            _anim.Play("Eyes_LookOut");
    }





}
