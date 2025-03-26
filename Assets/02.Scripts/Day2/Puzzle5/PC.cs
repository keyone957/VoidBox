using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC : MonoBehaviour
{
    public GameObject disk;

    MeshRenderer mesh;
    Material mat;
    float speed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mat = mesh.material;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Device")
        {
            mat.color = Color.blue;
            while (disk.transform.position.z < -1.3f)
            {
                disk.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }
    }
}
