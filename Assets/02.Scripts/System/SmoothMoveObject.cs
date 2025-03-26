using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMoveObject : MonoBehaviour
{
    [SerializeField] public Transform obj;
    [SerializeField] public Transform obj_base;
    public float duration =0.5f;

    private bool isMoving = false;

    public void ObjectRelease()
    {
        if (!isMoving)
        {
            StartCoroutine(ObjectMove());
        }
    }


    public IEnumerator ObjectMove()
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

 
}
