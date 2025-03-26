using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMoveObjectTarget : MonoBehaviour
{
    [SerializeField] public Transform obj;
    [SerializeField] public Transform obj_base;
    public float duration = 0.5f;

    private bool isMoving = false;

    // Ư�� ��ġ�� ��ü �̵�
    public void ObjectToTarget(Transform target)
    {
        if (!isMoving)
        {
            StartCoroutine(ObjectMoveTarget(target));
        }
    }

    // ���� ����� ObjectPull �� ��ġ�� �̵�
    public void ObjectToNearestObjectPull()
    {
        if (!isMoving)
        {
            Transform nearestBaseTransform = ObjectPullManager.FindNearestObjectPull(obj.position);
            if (nearestBaseTransform != null)
            {
                ObjectToTarget(nearestBaseTransform);
            }
        }
    }


    public IEnumerator ObjectMoveTarget(Transform obj_base)
    {
        isMoving = true;

        Vector3 startPos = obj.position;
        Quaternion startRot = obj.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < duration && Vector3.Distance(obj.position, obj_base.position) > 0.001f)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            obj.position = Vector3.Lerp(startPos, obj_base.position, t);
            obj.rotation = Quaternion.Lerp(startRot, obj_base.rotation, t);
            yield return null;
        }

        obj.rotation = obj_base.rotation;
        obj.position = obj_base.position;

        isMoving = false;
    }


    private ObjectPull FindNearestObjectPull()
    {
        ObjectPull[] objectPulls = FindObjectsOfType<ObjectPull>();
        ObjectPull nearest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = obj.position;

        foreach (ObjectPull objectPull in objectPulls)
        {
            float distance = Vector3.Distance(currentPosition, objectPull.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = objectPull;
            }
        }

        return nearest;
    }
}
