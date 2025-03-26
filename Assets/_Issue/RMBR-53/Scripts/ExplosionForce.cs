using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosionForce : MonoBehaviour
{
    public ForceMode forceMode = ForceMode.Impulse; // ���� ���
    public float force = 1.0f;
    [SerializeField] public bool hasExploded = false;      // ������ �̹� �߻��ߴ��� ����
    public GameObject wall;
    public Rigidbody[] peices;

    public void Explode()
    {
        wall.SetActive(false);
       
        foreach (var p in peices)
        {
            float x = Random.Range(-force, force);
            float y = Random.Range(0.0f, force);
            float z = Random.Range(-force, force);
            p.isKinematic = false;
            p.gameObject.SetActive(true);
            p.AddForce(new Vector3(x, y, z), forceMode);

        }
    }
}
