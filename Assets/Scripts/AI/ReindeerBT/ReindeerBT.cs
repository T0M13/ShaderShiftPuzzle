using System.Collections.Generic;
using BehaviorTree;
using UnityEngine.AI;

public class ReindeerBT : AnimalBT
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


    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new TaskIdle(agent, anim, centerPoint, idleRange, idleSpeed, idleAgentChangeDirModifier, IDLE_ANIMATION, IDLE_EMOTION),
        });

        return root;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
    }
}
