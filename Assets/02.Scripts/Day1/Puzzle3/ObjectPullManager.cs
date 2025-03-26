using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPullManager : MonoBehaviour
{

    public static List<Transform> allObjectPulls = new List<Transform>();

    public static void RegisterObjectPull(Transform objectPullTransform)
    {
        if (!allObjectPulls.Contains(objectPullTransform))
        {
            allObjectPulls.Add(objectPullTransform);
        }
    }

    public static void UnregisterObjectPull(Transform objectPullTransform)
    {
        if (allObjectPulls.Contains(objectPullTransform))
        {
            allObjectPulls.Remove(objectPullTransform);
        }
    }

    public static Transform FindNearestObjectPull(Vector3 position)
    {
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform objectPullTransform in allObjectPulls)
        {
            float distance = Vector3.Distance(position, objectPullTransform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = objectPullTransform;
            }
        }

        return nearest;
    }
}
