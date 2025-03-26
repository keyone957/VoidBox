using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using UnityEngine.VFX;
public class WristWatchUI : MonoBehaviour
{
    public static WristWatchUI Instance { get; private set; }

    [Header("Buttons")]
    [SerializeField] private GameObject buttons;
    [SerializeField] private InteractableUnityEventWrapper callButton;
    [SerializeField] private InteractableUnityEventWrapper inventoryButton;
    [SerializeField] private InteractableUnityEventWrapper settingButton;

    [Header("UI Objects")]
    [SerializeField] private GameObject uis;
    [SerializeField] private GameObject inventoryUIObj;
    [SerializeField] private GameObject settingUIObj;
    [SerializeField] private GameObject callUIObj;
    [SerializeField] private GameObject closeButton;
    public WristInventoryUI inventoryUI { get; private set; }
    private GameObject currentUI;

    [SerializeField] private VisualEffect visualEffect;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);

        Init();
    }

    private void Init()
    {
        callButton.WhenUnselect.AddListener(ShowCallUI);
        inventoryButton.WhenUnselect.AddListener(ShowInventoryUI);
        settingButton.WhenUnselect.AddListener(ShowSettingUI);
        closeButton.GetComponent<InteractableUnityEventWrapper>().WhenUnselect.AddListener(CloseUI);
        inventoryUI = inventoryUIObj.GetComponent<WristInventoryUI>();
        buttons.SetActive(false);
        uis.SetActive(false);
        Debug.Log("Init");
    }
    public void OnUI()
    {
        buttons.SetActive(true);
        uis.SetActive(false);
        Debug.Log("Call OnUI");
    }
    private void ShowUI(GameObject ui)
    {
        uis.SetActive(true);
        callUIObj.SetActive(false);
        settingUIObj.SetActive(false);
        inventoryUIObj.SetActive(false);

        ui.SetActive(true);
        currentUI = ui;
        Debug.Log("Show UI : " + ui.name);

        buttons.SetActive(false);
    }

    private void ShowInventoryUI() => ShowUI(inventoryUIObj);
    private void ShowSettingUI() => ShowUI(settingUIObj);
    private void ShowCallUI() => ShowUI(callUIObj);

    public void CloseUI()
    {
        // buttons.SetActive(true);
        uis.SetActive(false);

        visualEffect.Stop();
    }

    public void PlayCall(string name)
    {
        if (name is null) return;

        SoundManager.instance.PlaySound(name, SoundType.VOICE);
    }
}
