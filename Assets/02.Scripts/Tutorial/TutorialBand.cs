using System.Collections;
using UnityEngine;
using Oculus.Interaction;

public class TutorialBand : MonoBehaviour
{
    [SerializeField] private MemoryObject vrHeadSet;
    [SerializeField] private GameObject wristWatch;

    private Rigidbody rb;
    private SmoothMoveObject smoothMoveObject;
    private Transform playerWrist;
    public bool isContactPlayer;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void CheckContact(bool isContact)
    {
        if (isContact) isContactPlayer = true;
        else isContactPlayer = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isContactPlayer) return;
        if (other.CompareTag("Player") && other.name == "Wrist")
        {
            isContactPlayer = true;
            //name -> tag or component
            EquipWristWatch();
        }
    }
    private void EquipWristWatch()
    {
        if (isContactPlayer && TutorialManager.Instance.tutorialPhase == TutorialPhase.WristBand)
        {
            vrHeadSet.GetComponent<BoxCollider>().enabled = true;
            wristWatch.SetActive(true);

            TutorialManager.Instance.PhaseUpdate();

        }
        this.gameObject.SetActive(false);

    }
}
