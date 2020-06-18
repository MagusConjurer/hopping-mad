using System;
using System.Collections.Generic;
using UnityEngine;

public class _hoppingStar : MonoBehaviour {

    float sumX;
    float sumY;
    double sumXY;
    double root;
    float val;
    int min = 0;
    bool destined;
    [HideInInspector]
    public _waypoint destination;
    [HideInInspector]
    public _waypoint begin;
    List<_waypoint> openBucket;
    List<_waypoint> closedBucket;
    [HideInInspector]
    public List<_waypoint> theWay;
    List<_waypoint> revPath;
    _waypoint checking;

    public List<_waypoint> RunHoppingStar(_waypoint start, _waypoint end)
    {
        begin = start;
        destination = end;
        theWay = Connections(start, destination);
        if(theWay != null)
        {
            return theWay;
        }
        return null;
        
    }

    public float EvaluateDistance(_waypoint from, _waypoint to)
    {
        sumX = to.X - from.X;
        sumY = to.Y - from.Y;
        sumXY = Math.Pow(sumX, 2) + Math.Pow(sumY, 2);
        root = Mathf.Sqrt((float)sumXY);
        val = Convert.ToSingle(root);
        return val;
    }

    public void EvaluateWaypoint(_waypoint point)
    {
        if (point.Prev == null)
        {
            point.CSF = 0;
        }
        else
        {
            point.CSF = point.Prev.CSF + point.CSF;
        }
        point.H = EvaluateDistance(point, destination);
        point.TEC = point.CSF + point.H;
        
    }

    public List<_waypoint> Connections(_waypoint from, _waypoint to)
    {
        openBucket = new List<_waypoint>();
        closedBucket = new List<_waypoint>();

        EvaluateWaypoint(from);
        openBucket.Add(from);
        destined = true;
        while (openBucket.Count > 0 && destined)
        {
            for (int i = 0; i < openBucket.Count; i++)
            {
                float lowest = openBucket[min].TEC;
                if (openBucket.Count == 1)
                    min = 0;
                else
                {
                    if (openBucket[i].TEC <= lowest)
                        min = i;
                }
            }
            checking = openBucket[min];
            openBucket.Remove(checking);
            if (checking == to)
            {
                destined = false;
            }
            foreach (_waypoint waypoint in checking.Connected)
            {
                waypoint.Prev = checking;
                EvaluateWaypoint(waypoint);
                openBucket.Add(waypoint);
            }
            closedBucket.Add(checking);
        }
        return ThePath();
    }
    public List<_waypoint> ThePath()
    {
        revPath = new List<_waypoint>();
        foreach(_waypoint item in closedBucket)
        {
            revPath.Add(item);
        }
        return revPath;
    }
}
