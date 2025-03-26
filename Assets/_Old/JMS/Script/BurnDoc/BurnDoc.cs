using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnDoc : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int burnCount = 0;
    public bool fireOn = false;
    [SerializeField] private bool readyToBurn = false;
    [SerializeField] ParticleSystem fireParticle;
    [SerializeField] GameObject fire;
    [SerializeField] GameObject dumpsterFile;
    [SerializeField] GameObject[] subUIs;
    [SerializeField] private AudioSource _audioSoure;
    
    void Awake()
    {
        //PuzzleManager.Instance.burnDoc = this;
        fireParticle.Stop();
    }
    void init()
    {
        burnCount = 0;
    }
    public void PuzzleActivate()
    {
        //Activate Puzzle
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lighter") && fireOn)
        {
            readyToBurn = true;
            fireParticle.Play();
            _audioSoure.volume = 1;
        }

        if (other.gameObject.CompareTag("Document") && readyToBurn)
        {
            //other.gameObject.GetComponent<Grabbable>().enabled = false;
            other.gameObject.tag = "Untagged";
            burnCount++;
            subUIs[burnCount-1].SetActive(true);
            //document burning animation or transparent--
            other.transform.root.GetComponent<DissolveControl>().StartBuring(); 
        }
        if(burnCount == 3)
        {
            PuzzleManager.Instance.nextState = PuzzleManager.PuzzleState.OPENDOOR;
            dumpsterFile.SetActive(false);
            fireParticle.Stop();
            _audioSoure.volume = 0;
        }
    }

}
