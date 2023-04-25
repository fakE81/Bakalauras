using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static List<Vector3> waypoints = new List<Vector3>();
    public static List<Vector3> reversedWaypoints = new List<Vector3>();
    public Vector3 lastWaypoint;


    private void Start()
    {
        // First Waypoint.
        waypoints.Add(lastWaypoint);
    }

    public static void addWaypoint(Vector3 waypoint)
    {
        waypoints.Add(waypoint);
    }

    public static void ReverseWaypoints()
    {
        reversedWaypoints = new List<Vector3>(waypoints);
        reversedWaypoints.Reverse();
    }

    public static Vector3[] getWaypoints()
    {
        return reversedWaypoints.ToArray();
    }

    public static Vector3 getStartWaypoint()
    {
        return waypoints[waypoints.Count-1];
    }

    public static int getSize()
    {
        return reversedWaypoints.Count;
    }
}