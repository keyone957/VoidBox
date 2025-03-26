using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class ResetObjectPos : MonoBehaviour
{
    [SerializeField] public Transform obj_base;
    [SerializeField] private SnapInteractor snapInteractor;
    private Transform obj;
    public float duration = 0.5f;
    private bool isMoving = false;

    private void Start()
    {
        obj = GetComponent<Transform>();
    }

    public void ObjectToTarget()
    {
        if (!isMoving)
        {
            StartCoroutine(ObjectMoveTarget());
        }
    }

    private IEnumerator ObjectMoveTarget()
    {
        isMoving = true;
        snapInteractor.enabled = false;
        Vector3 startPos = gameObject.transform.position;
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
        snapInteractor.enabled = true;
        // snapInteractor.gameObject.SetActive(true);
    }
   
}
