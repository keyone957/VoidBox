using Oculus.Interaction;
using Oculus.Interaction.Input;
using System.Collections;
using UnityEngine;

public class Chip : MonoBehaviour
{
    //[SerializeField] private GlitchControl glitchControl;
 
    [SerializeField] private GameObject coffeeStick;
    [SerializeField] private MemoryObject memoryObject;
    [SerializeField] private GameObject[] lights;
    [SerializeField] private GameObject[] chipTexts;

    [SerializeField] private Material _material;

    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        memoryObject = coffeeStick.transform.GetComponentInChildren<MemoryObject>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Play();
            chipTexts[0].SetActive(true);
            coffeeStick.transform.GetComponent<BoxCollider>().enabled = true;
            Destroy(transform.parent.gameObject, 1);
            memoryObject.GetComponent<MeshRenderer>().material = _material;
            //memoryObject.StartText();
        }
    }
}
