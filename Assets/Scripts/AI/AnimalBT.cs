using System.Collections.Generic;
using BehaviorTree;
using UnityEngine.AI;


[UnityEngine.RequireComponent(typeof(NavMeshAgent), typeof(UnityEngine.Animator))]
public class AnimalBT : Tree
{

    [UnityEngine.Header("Agent Settings")]
    [UnityEngine.SerializeField] protected NavMeshAgent agent;
    [UnityEngine.Header("Animator Settings")]
    [UnityEngine.SerializeField] protected UnityEngine.Animator anim;

    [UnityEngine.Header("Idle Settings")]
    [UnityEngine.SerializeField] protected UnityEngine.Transform centerPoint;
    [UnityEngine.SerializeField] protected float idleRange = 30f;
    [UnityEngine.SerializeField] protected float idleSpeed = 8f;
    [UnityEngine.SerializeField] protected float idleAgentChangeDirModifier = 3f;
    public static UnityEngine.Vector3 pointToIdleTo;

    [UnityEngine.Header("Check Surrounding Settings")]
    [UnityEngine.SerializeField] protected float FOVRange = 10f;
    [UnityEngine.SerializeField] protected UnityEngine.LayerMask enemyLayerMask;
    public static bool checkForEnemy = true;
    [UnityEngine.SerializeField] protected float noCheckTime = 5f;

    [UnityEngine.Header("RunAway Settings")]
    [UnityEngine.SerializeField] protected float FOVRangeAddModifier = 2f;
    [UnityEngine.SerializeField] protected float runAwaySpeed = 18f;
    [UnityEngine.SerializeField] protected float runAwayDisplacementDist = 6f;
    [UnityEngine.SerializeField] protected float waitHidden = 3f;
    [UnityEngine.SerializeField] protected bool hide = false;
    [UnityEngine.SerializeField] protected UnityEngine.Transform[] hidePoints;
    public static UnityEngine.Vector3 pointToRunAwayTo;

    private void OnValidate()
    {
        if(agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (anim == null)
            anim = GetComponent<UnityEngine.Animator>();
    }

    protected virtual void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
        if (anim == null)
            anim = GetComponent<UnityEngine.Animator>();
    }

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new TaskCheckEnemyInFOVRange(transform, FOVRange, enemyLayerMask, noCheckTime),
                new TaskRunAway(transform, anim, agent, FOVRange, FOVRangeAddModifier, runAwaySpeed, runAwayDisplacementDist, hide, hidePoints, waitHidden),
            }),
            new TaskIdle(agent, anim, centerPoint, idleRange, idleSpeed, idleAgentChangeDirModifier),
        });

        return root;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        //Idle
        if (centerPoint != null)
        {
            UnityEngine.Gizmos.color = UnityEngine.Color.blue;
            UnityEngine.Gizmos.DrawWireSphere(centerPoint.position, idleRange);
        }
        if (pointToIdleTo != null)
        {
            UnityEngine.Gizmos.color = UnityEngine.Color.blue;
            UnityEngine.Gizmos.DrawSphere(pointToIdleTo, 1.0f);
        }

        //Check FOV Range
        UnityEngine.Gizmos.color = UnityEngine.Color.red;
        UnityEngine.Gizmos.DrawWireSphere(transform.position, FOVRange);
    }
}
