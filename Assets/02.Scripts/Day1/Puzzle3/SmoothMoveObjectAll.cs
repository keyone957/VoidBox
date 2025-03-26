using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMoveObjectAll : MonoBehaviour
{
    [SerializeField] List<SmoothMoveObject> smoothMoveObject;
    [SerializeField] List<SmoothMoveObjectTarget> smoothMoveObjectTargets;

    // use smoothMoveObject
    public void ObjectReleaseAll()
    {
        for (int i = 0; i < smoothMoveObject.Count; i++)
        {
            smoothMoveObject[i].ObjectRelease();
        }
    }

    // use smoothMoveObjectTargets
    public void MoveAllObjectsOnYAxis(float yOffset)
    {
        for (int i = 0; i < smoothMoveObjectTargets.Count; i++)
        {
            Vector3 targetPosition = smoothMoveObjectTargets[i].transform.position + new Vector3(0, yOffset, 0);
            Transform targetTransform = new GameObject("TargetTransform").transform;
            targetTransform.position = targetPosition;
            targetTransform.rotation = smoothMoveObjectTargets[i].transform.rotation;  

            smoothMoveObjectTargets[i].ObjectToTarget(targetTransform);
        }
    }
}
