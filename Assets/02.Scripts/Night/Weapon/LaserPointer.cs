using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private RaycastHit hit;

    [SerializeField] private Transform laserPoint;
    public Transform bulletPoint;
    private float raycastDistance = 100f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        transform.SetParent(WeaponCollection.instance.currentWeapon.transform);
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, bulletPoint.position);

        if (Physics.Raycast(bulletPoint.position, bulletPoint.forward * 3f, out hit, raycastDistance))
        {
            lineRenderer.SetPosition(1, hit.point);
            laserPoint.position = hit.point;
        }
        else
        {
            lineRenderer.SetPosition(1, bulletPoint.position + (bulletPoint.forward * raycastDistance));
            laserPoint.position = hit.point;
        }
    }
}