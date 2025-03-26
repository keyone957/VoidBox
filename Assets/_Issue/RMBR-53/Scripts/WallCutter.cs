using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCutter : MonoBehaviour
{
    public GameObject wallTarget;
    public float explosionForce = 200f;
    public float explosionRadius = 5f;
    public float delayBetweenPieces = 0.1f;
    public int rows = 16;
    public int columns = 16;
    public float delayBetweenRows = 0.1f;

    private List<List<GameObject>> wallPieces = new List<List<GameObject>>();

    public void FragmentWall()
    {
        if (wallTarget == null)
        {
            Debug.LogError("Wall target is not assigned.");
            return;
        }

        MeshFilter meshFilter = wallTarget.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter component missing from wall target.");
            return;
        }

        Vector3 planeSize = Vector3.Scale(meshFilter.mesh.bounds.size, wallTarget.transform.localScale);
        float pieceWidth = planeSize.x / columns;
        float pieceHeight = planeSize.z / rows;

        Vector3 startPosition = wallTarget.transform.position - wallTarget.transform.right * (planeSize.x / 2) - wallTarget.transform.forward * (planeSize.z / 2);

        for (int i = 0; i < rows; i++)
        {
            List<GameObject> rowPieces = new List<GameObject>();
            for (int j = 0; j < columns; j++)
            {
                GameObject piece = GameObject.CreatePrimitive(PrimitiveType.Plane);
                piece.transform.localScale = new Vector3(pieceWidth / 10, 1, pieceHeight / 10);

                Vector3 piecePosition = startPosition 
                    + wallTarget.transform.right * (i * pieceWidth + pieceWidth / 2) 
                    + wallTarget.transform.forward * (j * pieceHeight + pieceHeight / 2);
                piece.transform.position = piecePosition;
                piece.transform.rotation = wallTarget.transform.rotation;

                piece.GetComponent<MeshCollider>().convex = true;
                Rigidbody rb = piece.AddComponent<Rigidbody>();
                rb.isKinematic = true;
                piece.GetComponent<Renderer>().material = wallTarget.GetComponent<Renderer>().material;
                piece.name = wallTarget.name + "_Piece_Row_" + i + "_Col_" + j;
                piece.transform.parent = wallTarget.transform.parent;

                // Add individual collapse script to each pie

                rowPieces.Add(piece);
            }
            wallPieces.Add(rowPieces);
        }

        wallTarget.GetComponent<Renderer>().enabled = false;
        wallTarget.GetComponent<Collider>().enabled = false;
    }

    public void CollapseWall()
    {
        StartCoroutine(CollapseWallCoroutine());
    }

    IEnumerator CollapseWallCoroutine()
    {
        for (int i = 0; i < rows; i++)
        {
            List<GameObject> rowPieces = wallPieces[i];
            foreach(GameObject piece in rowPieces)
            {
                if (piece != null)
                {
                    Rigidbody rb = piece.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    }
                }
            }
            yield return null;
        }
    }


}


