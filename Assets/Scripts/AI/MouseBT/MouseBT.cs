using System.Collections.Generic;
using BehaviorTree;
using UnityEngine.AI;

public class MouseBT : AnimalBT
{
    [UnityEngine.Header("Cam Settings")]
    [UnityEngine.SerializeField] protected UnityEngine.GameObject mouseCam;

    [UnityEngine.Header("Animation Settings")]
    [UnityEngine.SerializeField] protected string IDLE_ANIMATION = "Walk";
    [UnityEngine.SerializeField] protected string IDLE_EMOTION = "Eyes_Happy";
    [UnityEngine.SerializeField] protected string CHASECHEESE_ANIMATION = "Walk";
    [UnityEngine.SerializeField] protected string CHASECHEESE_EMOTION = "Eyes_Happy";
    [UnityEngine.SerializeField] protected string EAT_ANIMATION = "Eat";
    [UnityEngine.SerializeField] protected string EAT_EMOTION = "Eyes_Excited";
    [UnityEngine.SerializeField] protected string RUNAWAY_ANIMATION = "Run";
    [UnityEngine.SerializeField] protected string RUNAWAY_EMOTION = "Eyes_LookOut";
    [UnityEngine.SerializeField] protected string HIDING_ANIMATION = "Idle_A";
    [UnityEngine.SerializeField] protected string HIDING_EMOTION = "Eyes_Trauma";

    [UnityEngine.Header("Cheese Settings")]
    [UnityEngine.SerializeField] private float cheeseRange = 8f;
    [UnityEngine.SerializeField] private float maxCheeseRange = 12f;
    [UnityEngine.SerializeField] private float eatRange = 1f;
    [UnityEngine.SerializeField] private float eatTime = 3f;
    [UnityEngine.SerializeField] private bool shouldEat = true;
    [UnityEngine.SerializeField] private UnityEngine.LayerMask cheeseMask;

    [UnityEngine.Header("After Cheese Settings")]
    [UnityEngine.SerializeField] protected float checkTaskRange = 15f;
    [UnityEngine.SerializeField] protected UnityEngine.LayerMask taskLayerMask;
    [UnityEngine.SerializeField] protected UnityEngine.Transform taskLocation;
    [UnityEngine.SerializeField] protected float maxInRangeTask = 12f;
    [UnityEngine.SerializeField] public static bool checkForTask = true;
    [UnityEngine.SerializeField] protected float noCheckTaskTime = 5f;
    [UnityEngine.SerializeField] protected float taskDoneRange = 1f;

    public float EatTime { get => eatTime; set => eatTime = value; }

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new TaskCheckDoTask(agent, anim, transform,  checkTaskRange, taskLayerMask, noCheckTaskTime),
                new TaskDoTask(agent, anim, transform, maxInRangeTask, taskDoneRange),
            }),
            new Sequence(new List<Node>
            {
                new TaskCheckCheeseInRange(transform, cheeseRange, cheeseMask),
                new TaskRunToCheese(transform, anim, agent, maxCheeseRange, eatRange, EatTime, shouldEat, CHASECHEESE_ANIMATION, CHASECHEESE_EMOTION, EAT_ANIMATION, EAT_EMOTION, IDLE_ANIMATION, IDLE_EMOTION),
            }),
            new Sequence(new List<Node>
            {
                new TaskCheckEnemyInFOVRange(transform, FOVRange, enemyLayerMask, noCheckTime),
                new TaskRunAway(transform, anim, agent, FOVRange, FOVRangeAddModifier, runAwaySpeed, runAwayDisplacementDist, hide, hidePoints, waitHidden, RUNAWAY_ANIMATION, RUNAWAY_EMOTION, HIDING_ANIMATION, HIDING_EMOTION, mouseCam),
            }),
            new TaskIdle(agent, anim, centerPoint, idleRange, idleSpeed, idleAgentChangeDirModifier, IDLE_ANIMATION, IDLE_EMOTION),
        });

        return root;
    }


    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        //Check Cheese Range
        UnityEngine.Gizmos.color = UnityEngine.Color.yellow;
        UnityEngine.Gizmos.DrawWireSphere(transform.position, cheeseRange);
        UnityEngine.Gizmos.color = UnityEngine.Color.green;
        UnityEngine.Gizmos.DrawWireSphere(transform.position, maxCheeseRange);
        UnityEngine.Gizmos.color = UnityEngine.Color.cyan;
        UnityEngine.Gizmos.DrawWireSphere(transform.position, eatRange);
        UnityEngine.Gizmos.color = UnityEngine.Color.magenta;
        UnityEngine.Gizmos.DrawWireSphere(transform.position, maxInRangeTask);
        UnityEngine.Gizmos.color = UnityEngine.Color.black;
        UnityEngine.Gizmos.DrawWireSphere(transform.position, taskDoneRange);
    }
}
