using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Dialog 정보
[System.Serializable]
public class Dialog
{
    public string name;
    [TextArea]
    public string text;
    public bool hasSound;
    public AudioSource voiceDub;
    public AudioClip voiceDubClip;
    public float scriptLength;
    public Transform dialogAnchor;
    public string nextscript;
}

public class DialogManager : MonoBehaviour
{
    public float testScriptLength;
    public static DialogManager instance { get; private set; }

    [SerializeField] private OVRCameraRig cameraRig;
    [SerializeField] private GameObject DialogWithSound;
    [SerializeField] private GameObject DialogNoSound;

    [Header("Dialogs")]
    [SerializeField] private List<Dialog> dialogs = new List<Dialog>();

    [Header("SystemMessage")]
    [SerializeField] private List<Dialog> systemMessages = new List<Dialog>();

    private Dictionary<string, GameObject> dialogsDict = new Dictionary<string, GameObject>();
    
    private void Awake()
    {
        if (instance == null) // Singleton
        {
            instance = this;
        }
        else Destroy(this.gameObject);

        InitScripts();
    }

    public void InitScripts()
    {
        dialogsDict.Clear();
        RegistDialogs();
    }

    private void RegistDialogs()
    {
        // 일단 Object를 생성
        foreach(var dialog in dialogs)
        {
            if(dialogsDict.ContainsKey(dialog.name))
            {
                Debug.Log(dialog.name + "가 이미 있습니다.");
                Debug.Log(dialog.name + " 등록 안 됨");
            }
            else
            {
                if(dialog.hasSound)
                {
                    dialogsDict.Add(dialog.name, Instantiate(DialogWithSound, dialog.dialogAnchor.position, dialog.dialogAnchor.rotation, this.transform));
                }
                else
                {
                    dialogsDict.Add(dialog.name, Instantiate(DialogNoSound, dialog.dialogAnchor.position, dialog.dialogAnchor.rotation, this.transform));
                }
            }
        }
        foreach(var message in systemMessages)
        {
            if(dialogsDict.ContainsKey(message.name))
            {
                Debug.Log(message.name + "가 이미 있습니다.");
                Debug.Log(message.name + " 등록 안 됨");
            }
            else
            {
                if(message.hasSound)
                {
                    dialogsDict.Add(message.name, Instantiate(DialogWithSound, message.dialogAnchor.position, message.dialogAnchor.rotation, this.transform));
                }
                else
                {
                    dialogsDict.Add(message.name, Instantiate(DialogNoSound, message.dialogAnchor.position, message.dialogAnchor.rotation, this.transform));
                }
            }
        }

        //한 번 더 돌면서 설정값을 맞춤(nextdialog를 object로 할당하기 위함)
        foreach(var dialog in dialogs)
        {
            SetDialog(GetDialog(dialog.name), dialog, true);
        }
        foreach(var message in systemMessages)
        {
            SetDialog(GetDialog(message.name), message, false);
        }
    }

    private GameObject GetDialog(string name)
    {
        if(!dialogsDict.ContainsKey(name))
        {
            Debug.Log(name + " 없음");
            return null;
        }
        return dialogsDict[name];
    }

    // Dialog의 내용을 바탕으로 prefab 구성
    public void SetDialog(GameObject dialogObject, Dialog dialog, bool isDialog)
    {
        if(dialog != null)
        {
            var scriptComponent = dialogObject.GetComponentInChildren<ScriptSoundMatch>();
            AudioSource audioSource = dialogObject.GetComponentInChildren<AudioSource>();
            if (scriptComponent != null)
            {
                scriptComponent.name = dialog.name;
                scriptComponent.textMeshPro.text = dialog.text;
                if(isDialog)
                {
                    scriptComponent.textMeshPro.fontStyle = FontStyles.Normal;
                }
                else
                {
                    scriptComponent.textMeshPro.fontStyle = FontStyles.Italic;
                }
                scriptComponent.hasSound = dialog.hasSound;
                audioSource = dialog.voiceDub;
                scriptComponent.voiceDub = dialog.voiceDub;
                scriptComponent.voiceClip = dialog.voiceDubClip;
                scriptComponent.scriptLength = dialog.scriptLength;
                if(dialog.nextscript != "")
                {
                    scriptComponent.nextscript = GetDialog(dialog.nextscript);
                }
            }
            else
            {
                Debug.Log("컴포넌트를 찾지 못하였습니다.");
            }
        }
    }

    // Dialog 출력
    public void ShowDialog(string name)
    {
        var dialog = GetDialog(name);
        if(dialog != null)
        {
            // 일단 둘 다 한 번에 활성화
            dialog.SetActive(true);
            var scriptComponent = dialog.transform.GetChild(0).GetComponent<ScriptSoundMatch>();

            if (scriptComponent != null && scriptComponent.hasSound)
            {
                // 오디오 소스가 있는 오브젝트도 활성화
                if (!scriptComponent.voiceDub.gameObject.activeInHierarchy)
                {
                    scriptComponent.voiceDub.gameObject.SetActive(true);
                }
            }
            scriptComponent.gameObject.SetActive(true);
            //dialog.GetComponentInChildren<ScriptSoundMatch>().gameObject.SetActive(true);
        }
    }
}