using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static List<Transform> waypoints = new List<Transform>();
    public static List<Transform> reversedWaypoints = new List<Transform>();
    public Transform lastWaypoint;


    private void Start()
    {
        // First Waypoint.
        waypoints.Add(lastWaypoint);
    }

    public static void addWaypoint(Transform waypoint)
    {
        waypoints.Add(waypoint);
        // Create reversed list.
        reversedWaypoints = new List<Transform>(waypoints);
        reversedWaypoints.Reverse();
    }

    public static Transform[] getWaypoints()
    {
        return reversedWaypoints.ToArray();
    }

    public static Transform getStartWaypoint()
    {
        return reversedWaypoints[0];
    }

    public static int getSize()
    {
        return reversedWaypoints.Count;
    }
}