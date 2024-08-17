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
    [SerializeField] private bool loop = false;
    [SerializeField] private bool onEndActivated = false;
    [SerializeField] private bool activateOnce = true;

    private Vector3 initialPosition; // Store initial position
    private Quaternion initialRotation; // Store initial rotation



    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        if (waypoints.Count > 0)
        {
            onStartPath.Invoke();
            isFollowingPath = true;
        }

        if (AudioManager.Instance)
        {
            AudioManager.Instance.PlaySound("ElectricLoopedLow", gameObject);
        }

    }

    private void OnEnable()
    {
        onStartPath.Invoke();
        isFollowingPath = true;
    }

    private void OnDisable()
    {
        isFollowingPath = false;
    }

    void Update()
    {
        if (isFollowingPath && currentWaypointIndex < waypoints.Count)
        {
            MoveTowardsWaypoint();
        }
        else if (isFollowingPath && currentWaypointIndex >= waypoints.Count)
        {
            if (!onEndActivated)
            {
                onEndPath.Invoke();
                onEndActivated = true;
            }
            isFollowingPath = false;
            if (destroyAtEnd)
            {
                Destroy(gameObject);
            }
        }

        if (loop && !isFollowingPath && currentWaypointIndex >= waypoints.Count && !destroyAtEnd)
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            currentWaypointIndex = 0;
            isFollowingPath = true;
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
    public void ResetPositionAndTransform() // Reset to initial position and rotation
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        currentWaypointIndex = 0;
        isFollowingPath = false;
        if (!activateOnce)
            onEndActivated = false;
    }
}
