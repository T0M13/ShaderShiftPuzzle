using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class PathFollower : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;       // List of waypoints to follow
    [SerializeField] private float speed = 1.0f;              // Movement speed
    [SerializeField] private bool destroyAtEnd = false;       // Flag to determine if the object should destroy itself at the end
    public UnityEvent onStartPath;          // Event triggered when the path following starts
    public UnityEvent onEndPath;            // Event triggered when the path following ends

    [SerializeField][ShowOnly] private int currentWaypointIndex = 0;   // Index of the current waypoint the GameObject is moving towards
    [SerializeField][ShowOnly] private bool isFollowingPath = false;   // Is the GameObject currently following the path?

    void Start()
    {
        if (waypoints.Count > 0)
        {
            onStartPath.Invoke();           // Trigger the start path event
            isFollowingPath = true;
        }
    }

    void Update()
    {
        if (isFollowingPath && currentWaypointIndex < waypoints.Count)
        {
            MoveTowardsWaypoint();
        }
        else if (isFollowingPath && currentWaypointIndex >= waypoints.Count)
        {
            onEndPath.Invoke();            // Trigger the end path event
            isFollowingPath = false;       // Stop path following
            if (destroyAtEnd)
            {
                Destroy(gameObject);
            }
        }
    }

    void MoveTowardsWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 movementDirection = targetWaypoint.position - transform.position;
        float step = speed * Time.deltaTime;

        if (movementDirection.magnitude <= step)
        {
            // Reached the waypoint
            transform.position = targetWaypoint.position;
            currentWaypointIndex++;  // Move to the next waypoint
        }
        else
        {
            // Move towards the waypoint
            transform.position += movementDirection.normalized * step;
        }
    }
}
