using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject GUI;
    public GameObject LoadGUI;
    public GameObject SettingGUI;

    public string[] SceneName;
    public AudioMixer audioMixer;

    public float frequency;
    public float amplitude;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void StartButton()
    {
        GUI.active = false;
        SceneManager.LoadScene("TutorialScene");
    }
    public void LoadButton()
    {
        LoadGUI.active = true;
    }
    public void ClickonSceneButton(int i)
    {
        LoadGUI.active = false;
        GUI.active = false;
        SceneManager.LoadScene(SceneName[i]);
    }

    public void ClickSceneButtonOnWave(int WaveNum)
    {
        LoadGUI.active = false;
        GUI.active = false;
        SceneDataManager.instance.WaveNum = WaveNum;
        SceneManager.LoadScene("NightScene1105");
    }

    public void SettingButton()
    {
        SettingGUI.active = true;
    }
    public void ClickonSettingButton()
    {
        SettingGUI.active = false;
    }
    public void QuitButton()
    {
        GUI.active = false;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    public void SetGlobalBrightness(float value)
    {
        // // intensity 범위를 적절하게 설정 (0 ~ 1)
        RenderSettings.ambientIntensity = value;
    }

    public void SetGlobalVibration(float value)
    {
        OVRInput.Controller controller = OVRInput.Controller.RTouch;
        frequency = value;
        amplitude = value;
        OVRInput.SetControllerVibration(frequency, amplitude, controller);
    }
    public void SetBGMVolume(float volume)
    {
        // 볼륨을 데시벨 단위로 변환 (예: -80dB에서 0dB)
        float dBValue = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1)) * 20;
        audioMixer.SetFloat("BGM", dBValue);
    }
    public void SetSFXVolume(float volume)
    {
        // 볼륨을 데시벨 단위로 변환 (예: -80dB에서 0dB)
        float dBValue = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1)) * 20;
        audioMixer.SetFloat("SFX", dBValue);
    }
}
