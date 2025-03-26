using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoryObject : MonoBehaviour
{
    [SerializeField] private GameObject[] rushText;
    [SerializeField] private GameObject sceneChangeText;
    [SerializeField] private GameObject cameraRig;
    [SerializeField] private Transform centerEye;
    [SerializeField] private FadeScreen fadeScreen;

    [SerializeField] private ScreenTransitionManager screenTransitionManager;
    public bool textCheck;
    public bool isContactPlayer;
    public bool equipWristBand = true;

    public void StartText()
    {
        StartCoroutine(RushText());
    }
    public void OnHover()
    {
        if (TutorialManager.Instance.tutorialPhase == TutorialPhase.VRHeadSet) this.GetComponent<BoxCollider>().enabled = true;
        else
        {
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }
    public void UnHover()
    {
        this.GetComponent<BoxCollider>().enabled = true;
    }
    public void UnSelect()
    {
        if (isContactPlayer) StartCoroutine(SceneLoad());
        else
        {
            Debug.Log("VRH");
            //DialogManager.instance.ShowDialog("VRHeadSet");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isContactPlayer = true;
        //if (TutorialManager.Instance.tutorialPhase != TutorialPhase.VRHeadSet) DialogManager.instance.ShowDialog("WristBand");
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isContactPlayer = false;
    }
    IEnumerator RushText()
    {
        float activeTimer = 5f;
        while (true)
        {
            if (textCheck)
            {
                GameObject tmp = rushText[Random.Range(0, rushText.Length)].gameObject;
                tmp.SetActive(true);

                yield return new WaitForSeconds(activeTimer);
                tmp.SetActive(false);
            }
            else yield return null;
        }
    }
    private IEnumerator SceneLoad()
    {
        ObjContact();
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync(2);
        this.gameObject.SetActive(false);
    }
    void ObjContact()
    {
        cameraRig.transform.position = 
            Vector3.MoveTowards(cameraRig.transform.position,Vector3.forward, Time.deltaTime * 0.2f);
    }
}