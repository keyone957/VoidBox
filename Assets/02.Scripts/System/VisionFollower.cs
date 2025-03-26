
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionFollower : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float maxDistanceFromCamera = 3.0f;
    [SerializeField] private float moveSpeed = 0.5f;

    // Built-in Unity
    private void Update()
    {
        float distanceFromCamera = Vector3.Distance(transform.position, cameraTransform.position);
        if (distanceFromCamera > maxDistanceFromCamera)
        {
            MoveTowards(cameraTransform.position + (cameraTransform.forward * maxDistanceFromCamera));
        }
    }

    private void MoveTowards(Vector3 targetPosition)
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }
}