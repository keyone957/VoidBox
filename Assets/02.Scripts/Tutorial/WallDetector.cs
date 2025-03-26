using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    public GameObject ovrCamera;

    private LayerMask layer = 1 << 6;

    public List<Wall> walls = new List<Wall>();

    public static Wall farthestWall; //need modification
    void Start()
    {
        WallDetecting();
    }

    public void WallDetecting()
    {
        Vector3 ovrPos = ovrCamera.transform.position;
        Quaternion ovrRot = ovrCamera.transform.rotation;
        for (int i = 0; i < 4; i++)
        {
            RaycastHit hit;
            Vector3 direction = ovrRot * Quaternion.Euler(0, i * 90, 0) * Vector3.forward;

            if (Physics.Raycast(ovrPos, direction, out hit, Mathf.Infinity, layer))
            {
                if (farthestWall == null) farthestWall = hit.transform.GetComponent<Wall>();

                walls.Add(hit.transform.GetComponent<Wall>());

                float distanceToWall = Vector3.Distance(ovrCamera.transform.position, farthestWall.transform.position);
                if (distanceToWall < Vector3.Distance(ovrCamera.transform.position, hit.transform.position))
                {
                    farthestWall = hit.transform.GetComponent<Wall>();
                }
            }
        }
    }
}
