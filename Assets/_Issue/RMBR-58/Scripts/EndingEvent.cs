using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using Unity.VisualScripting;

public class EndingEvent : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private FadeScreen fadeScreen;
    [SerializeField] private GameObject dummyEndText;
    public static EndingEvent instance { get; private set; }


    public UniversalAdditionalCameraData cameraCen;
    public LightController lightController; // ����Ʈ ��Ʈ�ѷ�
    public DoorController doorController; // �� ��Ʈ�ѷ�
    public ButtonLight vrLightController; // VR ������ ��Ʈ�ѷ�
    public GameObject modifiedVRHeadset; // ������ VR ���� ������Ʈ
    public AudioSource voiceAudio; // ���� ��Ҹ� �����
    public AudioClip promptClip; // "�ٽ� �Ӹ��� ��!" ���� Ŭ��

    [SerializeField] private bool isPlayerHoldingVR = false;
    [SerializeField] private bool isPlayerWearingVR = false;

    private void Awake()
    {
        if (instance == null) // Singleton
        {
            instance = this;
        }
        else Destroy(this.gameObject);

    }
    void Update()
    {
        if (!isPlayerHoldingVR || !isPlayerWearingVR)
        {
            // �÷��̾ VR�� �������� ������ ���� ���
            voiceAudio.PlayOneShot(promptClip);
        }
        else if (isPlayerWearingVR)
        {
            //StartCoroutine(GameFinished());
        }

    }

    public void ConvertCombatEnv()
    {
        StartCoroutine(CombatStartSequentially());
    }

    private IEnumerator CombatStartSequentially()
    {
        yield return new WaitForSeconds(3.0f);

        lightController.StartTurningOnLights();

        yield return new WaitForSeconds(4.0f);

        doorController.OpenDoors();
        // cameraCen.renderPostProcessing = true;

    }

    public void PlayEnding()
    {
        StartCoroutine(PlayEndingSequence());
    }

    private IEnumerator PlayEndingSequence()
    {
        // �� ������
        yield return new WaitForSeconds(2f); // ���� ���� �ð��� ���

        doorController.CloseDoors();
        yield return new WaitForSeconds(2f); // ���� ���� �ð��� ���

        // ����Ʈ ���� ����
        lightController.StartTurningOffLights();
        yield return new WaitForSeconds(lightController.delayBetweenLights * lightController.spotlights.Length + 1f);

        modifiedVRHeadset.SetActive(true);
        modifiedVRHeadset.GetComponent<Rigidbody>().isKinematic = false;
        vrLightController.ToggleBlinking(true);

        yield return new WaitForSeconds(3f);
        modifiedVRHeadset.GetComponent<Rigidbody>().isKinematic = true;


        //// Thank you for Playing ^^ �޼��� ���� (Ÿ��Ʋ ������ �̵�)
        //ShowThankYouMessage();
        //yield return new WaitForSeconds(3f);

        //// Ÿ��Ʋ ������ �̵�
        //LoadTitleScene();
    }
    public void ShowEndGameMessage()
    {
        StartCoroutine(EndRoutine());
    }
    private IEnumerator EndRoutine()
    {
        fadeScreen.fadeDuration = 2f;
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(fadeScreen.fadeDuration + 1f);

        ShowThankYouMessage();

        yield return new WaitForSeconds(3.0f);

        LoadTitleScene();
    }

    public void ShowThankYouMessage()
    {
        dummyEndText.SetActive(true);
        Debug.Log("Thank you for Playing ^^");
    }

    private void LoadTitleScene()
    {
        // Ÿ��Ʋ �� �ε� (����Ƽ�� �� �Ŵ��� ���)
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
    }
}
