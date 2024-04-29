using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIMouse : MonoBehaviour
{
    [Header("Agent Settings")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform centerPoint;
    [SerializeField] private float agentChangeDirModifier = .5f;

    [Header("State Settings")]
    [SerializeField] private MouseState currentMouseState;
    [Header("Surrounding Checking Settings")]
    [SerializeField] private bool checkSurroundings = true;
    [SerializeField] private float checkRange = 10f;


    [Header("Idle Settings")]
    [SerializeField] private float idleRange = 15f;
    [SerializeField] private float idleSpeed = 5f;
    [SerializeField] private Vector3 pointToIdleTo;

    [Header("RunAway Settings")]
    [SerializeField] private Transform chaser;
    [SerializeField] private Transform[] runAwayPositions;
    [SerializeField] private float displacementDist = 1f;
    //[SerializeField] private float waitTillIdle = 3f;
    [SerializeField] private float runAwaySpeed = 13f;
    [SerializeField] private Coroutine runAwayCoroutine;
    [SerializeField] private Vector3 pointToRunAwayTo;


    private void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        currentMouseState = MouseState.Idle;
    }

    private void Update()
    {
        if (agent == null) return;

        switch (currentMouseState)
        {
            case MouseState.Idle:
                MouseIdle();
                break;
            case MouseState.RunAway:
                MouseRunAway();
                break;
            case MouseState.GetCheese:
                break;
        }
        CheckSurrounding();


    }
    private void MouseIdle()
    {

        if (agent.remainingDistance <= agent.stoppingDistance + agentChangeDirModifier)
        {
            Vector3 point;
            if (RandomPoint(centerPoint.position, idleRange, out point))
            {
                agent.speed = idleSpeed;
                pointToIdleTo = point;
                MoveToPos(point);
            }
        }
    }

    private void MouseRunAway()
    {
        if (chaser == null) return;
        if (runAwayPositions.Length > 0)
        {
            if (agent.remainingDistance <= agent.stoppingDistance + agentChangeDirModifier)
            {
                Vector3 point;
                if (RandomRunAwayPoint(out point))
                {
                    agent.speed = runAwaySpeed;
                    pointToIdleTo = point;
                    MoveToPos(point);
                }
            }
        }
        else
        {
            float dist = Vector3.Distance(chaser.transform.position, transform.position);
            if (dist < checkRange)
            {
                agent.speed = runAwaySpeed;
                Vector3 normDir = (chaser.position - transform.position).normalized;
                pointToRunAwayTo = (transform.position - (normDir * displacementDist));
                MoveToPos(pointToRunAwayTo);
            }
            else
            {
                checkSurroundings = true;
                currentMouseState = MouseState.Idle;
                Debug.Log("New State: " + currentMouseState);
                chaser = null;
            }
        }

    }

    private void MoveToPos(Vector3 pos)
    {
        agent.SetDestination(pos);
    }

    private void CheckSurrounding()
    {
        if (!checkSurroundings) return;
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, checkRange, Vector3.up);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.GetComponent<Health>() != null)
            {
                checkSurroundings = false;
                chaser = hit.transform;
                Debug.Log("New Chaser: " + chaser.name);
                currentMouseState = MouseState.RunAway;
                Debug.Log("New State: " + currentMouseState);
            }
        }
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

    private bool RandomRunAwayPoint(out Vector3 randomRunAwayPoint)
    {
        Transform randomP = runAwayPositions[Random.Range(0, runAwayPositions.Length - 1)];
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomP.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            randomRunAwayPoint = hit.position;
            return true;
        }

        randomRunAwayPoint = Vector3.zero;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(centerPoint.position, idleRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(pointToIdleTo, 1.0f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pointToRunAwayTo, 1.0f);
        Gizmos.DrawWireSphere(transform.position, checkRange);
    }

    private enum MouseState
    {
        Idle,
        RunAway,
        GetCheese
    }

}
