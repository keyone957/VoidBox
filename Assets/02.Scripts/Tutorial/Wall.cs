using UnityEngine;

public class Wall : MonoBehaviour
{
    public Vector3 RandomPortalPosition(GameObject obj)
    {
        Vector3 wallPos = this.transform.position;
        Vector3 wallRight = this.transform.right;
        float halfWallWidth = this.transform.GetComponent<MeshRenderer>().bounds.size.x / 2;

        float portalOffset = Random.Range(-halfWallWidth, halfWallWidth);

        Vector3 spawnPos = wallPos + wallRight * portalOffset;
        spawnPos.y = obj.transform.localScale.y / 2;

        return spawnPos;
    }
}
