using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    MeshRenderer mesh;
    Material mat;

    private AudioSource audioSource;
    public AudioClip sirenSound;

    [SerializeField] Light fieldLight;
    [SerializeField] Color fieldColor;


    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mat = mesh.material;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1;
        audioSource.loop = true;
        audioSource.volume = 0.1f;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "fire")
            mat.color = Color.red;
        else if (collider.gameObject.tag == "Disk")
        {
            audioSource.PlayOneShot(sirenSound);
            while (fieldLight.color != fieldColor)
            {
                fieldLight.color = Color.Lerp(fieldLight.color, fieldColor, 0.1f);
            }
        }
    }
}
