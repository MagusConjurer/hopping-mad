using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(_hoppingStar))]
public class _waypoint : MonoBehaviour {
    [HideInInspector]
    public float X;
    [HideInInspector]
    public float Y;
    // cost so far
    [HideInInspector]
    public float CSF;
    // heuritic (cost to final destination
    [HideInInspector]
    public float H;
    // total estimated cost (previous two combined)
    [HideInInspector]
    public float TEC;
    [HideInInspector]
    public _waypoint Prev = null;
    public List<_waypoint> Connected;

    _waypoint closest;
    Vector3 currentPosition;
    Vector3 directionToTarget;
    float closestDistance;
    float distanceToTarget;
    private void Start()
    {
        X = transform.position.x;
        Y = transform.position.y;
    }
    private void Update()
    {
        X = transform.position.x;
        Y = transform.position.y;
    }
    public _waypoint GetClosestWaypoint(GameObject[] waypoints)
    {
        closest = null;
        closestDistance = Mathf.Infinity;
        currentPosition = transform.position;
        foreach (GameObject potentialTarget in waypoints)
        {
            directionToTarget = potentialTarget.transform.position - currentPosition;
            distanceToTarget = directionToTarget.sqrMagnitude;
            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                closest = potentialTarget.GetComponent<_waypoint>();
            }
        }
        return closest;
    }
}
