using System.Collections;
using UnityEngine;

public class TutorialBox : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] renderers;

    [Header("±â¹Í °ü·Ã")]
    [SerializeField] private GameObject vrHeadSet;
    [SerializeField] private GameObject memo;
    [SerializeField] private GameObject dummyBand;

    [Header("objPosition")]
    [SerializeField] private Transform vrHeadSetPosition;
    [SerializeField] private Transform memoPosition;
    [SerializeField] private MemoryObject memoryObject;
    [SerializeField] private Transform dummyBandPosition;

    [Space]
    [SerializeField] private FadeScreen fadeScreen;
    [SerializeField] private Transform player;  //centerEye
    [SerializeField] private GameObject boxLid;
    [SerializeField] private TutorialDoor door;
    [SerializeField] private GameObject table;
    [SerializeField] private Transform dummyFloor;
    [SerializeField] private AudioClip fallClip;
    [SerializeField] private Transform tutorial_GripPos;

    private Floor floor;
    private Rigidbody lidRb;
    private Rigidbody rb;
    private Animator animator;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        lidRb = boxLid.GetComponent<Rigidbody>();
    }
    private void Start()
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = false;
        }
        StartCoroutine(SpawnBox());
    }
    public IEnumerator SpawnBox()
    {
        fadeScreen.gameObject.SetActive(true);
        rb.isKinematic = true;
        lidRb.isKinematic = true;
        door.GetComponent<Rigidbody>().isKinematic = true;

        yield return new WaitForSeconds(1);

        TutorialManager.Instance.PhaseUpdate();
        floor = GameObject.FindAnyObjectByType<Floor>();

        door.SpawnDoor(player.position + (Vector3.forward * 0.5f) + Vector3.up);
        table.transform.position = door.transform.position - (Vector3.forward * 0.05f) + (Vector3.down * 0.8f);

        fadeScreen.FadeIn();
        door.GetComponent<Rigidbody>().isKinematic = false;

        while (TutorialManager.Instance.tutorialPhase != TutorialPhase.BoxOpen) yield return null;

        //this.transform.position = Vector3.Lerp(player.position, door.transform.position, 0.8f) + Vector3.up;
        this.transform.position = Vector3.Lerp(player.position, door.transform.position, 0.9f) + (Vector3.up * 0.35f);

        rb.isKinematic = false;

        foreach (var renderer in renderers)
        {
            renderer.enabled = true;
        }
    }
    public void OpenBoxlid()
    {
        lidRb.useGravity = true;
        lidRb.isKinematic = false;
        TutorialUI.Instance.stopCoroutine = true;
        TutorialManager.Instance.PhaseUpdate();
        //memoryObject.StartText();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("GlobalMesh"))
        {
            rb.isKinematic = true;
            AudioSource.PlayClipAtPoint(fallClip, transform.position);
            OnGrounded();
        }
    }
    private void OnGrounded()
    {
        TutorialUI.Instance.Init();
        animator.SetTrigger("IsGrounded");
        TutorialUI.Instance.ShowImage(tutorial_GripPos, "Grip", true);
        StartCoroutine(GroundedCheckCoroutine());
    }
    private IEnumerator GroundedCheckCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        rb.isKinematic = true;

        yield return null;
        vrHeadSet.SetActive(true);
        memo.SetActive(true);
        dummyBand.SetActive(true);
        vrHeadSet.transform.position = vrHeadSetPosition.position;
        memo.transform.position = memoPosition.position;
        dummyBand.transform.position = dummyBandPosition.position;
    }
}
