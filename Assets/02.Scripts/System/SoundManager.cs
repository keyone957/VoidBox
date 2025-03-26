using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    BGM,
    SFX,
    VOICE,
    Ambient,
    MaxCount
}

//audioClip 정보
[System.Serializable]
public class AudioClips
{
    public string name;
    public AudioClip audioClip;
}
[System.Serializable]
public struct AmbientSet
{
    public float volume;
    public float minDistance;
    public float maxDistance;
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }

    public AudioMixer audioMixer;

    [SerializeField] private GameObject audioPrefab;
    [SerializeField] private AudioSource bgmAudioSource;
    private AudioMixerGroup[] audioMixerGroup;

    //ambient
    private List<AudioSource> ambientSource = new List<AudioSource>();

    //SFX
    private List<AudioSource> audioSources = new List<AudioSource>();

    //이름과 Clip등록
    [Header("AudioClips")]
    [SerializeField] private List<AudioClips> bgmClips = new List<AudioClips>();
    [SerializeField] private List<AudioClips> SFXClips = new List<AudioClips>();
    [SerializeField] private List<AudioClips> VOICEClips = new List<AudioClips>();
    [SerializeField] private List<AudioClips> ambientClips = new List<AudioClips>();

    private Dictionary<string, AudioClips> audioSourceClips = new Dictionary<string, AudioClips>();
    public Dictionary<string, AudioClips> _audioSourceClips => audioSourceClips;
    private Dictionary<string, AudioMixerGroup> audioMixerGroupPairs = new Dictionary<string, AudioMixerGroup>();

    private float dummyBGMVolume = 0.3f;
    private float dummyVolume = 0.5f;

    private bool startStopRoutine;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(this.gameObject);

        InitAudios();
    }
    public void InitAudios()
    {
        audioSources.Clear();
        audioSourceClips.Clear();
        GetAudioSource();
        audioMixerGroup = audioMixer.FindMatchingGroups("Master");
        foreach (var mixer in audioMixerGroup)
        {
            audioMixerGroupPairs.Add(mixer.name, mixer);
        }
        foreach (var audio in audioSources)
        {
            audio.Stop();
            audio.clip = null;
            audio.loop = false;
        }
        bgmAudioSource.volume = dummyBGMVolume;
        RegistAudioClips();
    }

    private void RegistAudioClips()
    {
        foreach (var clips in bgmClips)
        {
            if (audioSourceClips.ContainsKey(clips.name))
            {
                Debug.Log(clips.name + "가 이미 있습니다.");
                Debug.Log(clips.audioClip.name + "등록 안 됨");
            }
            else
            {
                audioSourceClips.Add(clips.name, clips);
            }
        }
        foreach (var clips in SFXClips)
        {
            if (audioSourceClips.ContainsKey(clips.name))
            {
                Debug.Log(clips.name + "가 이미 있습니다.");
                Debug.Log(clips.audioClip.name + "등록 안 됨");
            }
            else audioSourceClips.Add(clips.name, clips);
        }
        foreach (var clips in ambientClips)
        {
            if (audioSourceClips.ContainsKey(clips.name))
            {
                Debug.Log(clips.name + "가 이미 있습니다.");
                Debug.Log(clips.audioClip.name + "등록 안 됨");
            }
            else audioSourceClips.Add(clips.name, clips);
        }
    }
    private AudioClip GetAudioClip(string name)
    {
        if (!audioSourceClips.ContainsKey(name))
        {
            Debug.Log(name + " 없음");
            return null;
        }
        return audioSourceClips[name].audioClip;
    }
    private AudioSource GetAudioSource()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                audioSources[i].volume = dummyVolume; //needTest
                return audioSources[i];
            }
        }
        GameObject audioSourceObject = Instantiate(audioPrefab, transform);
        AudioSource newAudioSource = audioSourceObject.GetComponent<AudioSource>();
        newAudioSource.volume = dummyVolume; //needTest
        audioSources.Add(newAudioSource);
        return newAudioSource;
    }

    #region playSound -> 기본 재생
    /// <summary>
    /// 일반적인 BGM, SFX재생 시 사용
    /// type별로 추가 기능은 미정 -> ambient?
    /// </summary>
    /// <param name="type"> sound 타입 </param>
    /// <param name="isLoop"> 반복 여부 </param>
    public void PlaySound(string name, SoundType type, bool isLoop = false)
    {
        var audioSource = new AudioSource();
        if (type == SoundType.BGM)
        {
            audioSource = bgmAudioSource;
            audioSource.outputAudioMixerGroup = audioMixerGroupPairs["BGM"];
            audioSource.loop = isLoop;
        }
        else if (type == SoundType.SFX)
        {
            audioSource = GetAudioSource();
            audioSource.outputAudioMixerGroup = audioMixerGroupPairs["SFX"];
            audioSource.loop = isLoop;
        }
        audioSource.volume = dummyVolume;

        audioSource.clip = GetAudioClip(name);
        audioSource.Play();
    }
    /// <summary>
    /// 일반적인 BGM, SFX재생 시 사용
    /// type별로 추가 기능은 미정 -> ambient?
    /// </summary>
    /// <param name="type"> sound 타입 </param>
    /// <param name="isLoop"> 반복 여부 </param>
    /// <param name="volume"> 볼륨 세팅 구현 이전 사용 </param>

    public void PlaySound(string name, SoundType type, float volume, bool isLoop = false)
    {
        var audioSource = new AudioSource();
        switch (type)
        {
            case SoundType.BGM:
                audioSource = bgmAudioSource;
                audioSource.outputAudioMixerGroup = audioMixerGroupPairs["BGM"];
                audioSource.loop = isLoop;
                break;
            case SoundType.SFX:
                audioSource = GetAudioSource();
                audioSource.outputAudioMixerGroup = audioMixerGroupPairs["SFX"];
                audioSource.loop = isLoop;
                break;
        }
        audioSource.clip = GetAudioClip(name);
        audioSource.volume = volume;
        audioSource.Play();
    }
    /// <summary>
    /// 일반적인 BGM, SFX재생 시 사용
    /// type별로 추가 기능은 미정 -> ambient?
    /// </summary>
    /// <param name="type"> sound 타입 </param>
    /// <param name="isLoop"> 반복 여부 </param>
    /// <param name="startTime"> 사운드 시작 시간</param>
    public void DelayPlaySound(string name, SoundType type, float startTime, bool isLoop = false)
    {
        StartCoroutine(DelayPlayRoutine(name, type, startTime, isLoop));
    }
    private IEnumerator DelayPlayRoutine(string name, SoundType type, float startTime, bool isLoop = false)
    {
        yield return new WaitForSeconds(startTime);

        var audioSource = new AudioSource();
        switch (type)
        {
            case SoundType.BGM:
                audioSource = bgmAudioSource;
                audioSource.outputAudioMixerGroup = audioMixerGroupPairs["BGM"];
                audioSource.loop = isLoop;
                break;
            case SoundType.SFX:
                audioSource = GetAudioSource();
                audioSource.outputAudioMixerGroup = audioMixerGroupPairs["SFX"];
                audioSource.loop = isLoop;
                break;
        }
        audioSource.clip = GetAudioClip(name);
        audioSource.Play();
    }
    public void PlayAmbientSound(AudioSource source, string name, AmbientSet ambientSet, bool isLoop = true)
    {
        source.spatialBlend = 1.0f;
        source.volume = ambientSet.volume;
        source.minDistance = ambientSet.minDistance;
        source.maxDistance = ambientSet.maxDistance;
        source.loop = isLoop;
        source.clip = GetAudioClip(name);
        source.outputAudioMixerGroup = audioMixerGroupPairs["Ambient"];
        source.Play();
    }

    /// <summary>
    /// event등록용 (SFX)
    /// </summary>
    /// <param name="name"> clip name </param>
    public void SimplePlaySFX(string name)
    {
        PlaySound(name, SoundType.SFX);
    }
    #endregion

    #region StopSound
    public void ClearAllSound()
    {
        StopSoundByType(SoundType.BGM);
        StopSoundByType(SoundType.SFX);
    }
    public void StopSoundByType(SoundType type)
    {
        switch (type)
        {
            case SoundType.BGM:
                bgmAudioSource.Stop();
                break;
            case SoundType.SFX:
                foreach (var audioSource in audioSources)
                {
                    if (audioSource.isPlaying)
                    {
                        audioSource.Stop();
                    }
                }
                break;
            case SoundType.VOICE:
                foreach (var audioSource in audioSources)
                {
                    if (audioSource.isPlaying)
                    {
                        audioSource.Stop();
                    }
                }
                break;
        }
    }
    public void StopSoundByClip(string name)
    {
        AudioClip audioClip = GetAudioClip(name);
        foreach (var audioSource in audioSources)
        {
            if (audioSource.isPlaying && audioSource.clip == audioClip)
            {
                audioSource.Stop();
            }
        }
    }
    #endregion

    #region FadeInOutSound
    public void FadeInBGM(float duration)
    {
        StartCoroutine("OnFadeInBGM", duration);
    }

    private IEnumerator OnFadeInBGM(float duration)
    {
        var audioSource = bgmAudioSource;

        audioSource.volume = 0f;
        float targetVolume = Mathf.Pow(10, dummyBGMVolume / 20f);

        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += Time.deltaTime / duration * targetVolume;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    public void FadeOutBGM(float duration)
    {
        StartCoroutine("OnFadeOutBGM", duration);
    }
    private IEnumerator OnFadeOutBGM(float duration)
    {
        var audioSource = bgmAudioSource;
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0f)
        {
            audioSource.volume -= Time.deltaTime / duration * startVolume;
            yield return null;
        }
        audioSource.volume = 0f;
        audioSource.Stop();
        audioSource.volume = Mathf.Pow(10, dummyVolume / 20f);
    }
    public void FadeOutSFX(string clipName, float duration)
    {
        StartCoroutine(OnFadeOutSFX(clipName, duration));
    }
    private IEnumerator OnFadeOutSFX(string clipName, float duration)
    {
        var audioClip = GetAudioClip(clipName);
        var audioSourceTmp = new AudioSource();
        foreach (var audioSource in audioSources)
        {
            if (audioSource.isPlaying && audioSource.clip == audioClip)
            {
                audioSourceTmp = audioSource;
            }
        }
        float startVolume = audioSourceTmp.volume;

        while (audioSourceTmp.volume > 0f)
        {
            audioSourceTmp.volume -= Time.deltaTime / duration * startVolume;
            yield return null;
        }

        audioSourceTmp.volume = 0f;
        audioSourceTmp.Stop();
        audioSourceTmp.volume = Mathf.Pow(10, dummyVolume / 20f);
    }
    public void FadeInSFX(string clipName, float duration)
    {
        StartCoroutine(OnFadeInSFX(clipName, duration));
    }
    private IEnumerator OnFadeInSFX(string clipName, float duration)
    {
        var audioClip = GetAudioClip(clipName);
        var audioSourceTmp = new AudioSource();
        foreach (var audioSource in audioSources)
        {
            if (audioSource.isPlaying && audioSource.clip == audioClip)
            {
                audioSourceTmp = audioSource;
            }
        }
        float startVolume = audioSourceTmp.volume;

        while (audioSourceTmp.volume > 0f)
        {
            audioSourceTmp.volume += Time.deltaTime / duration * startVolume;
            yield return null;
        }

        audioSourceTmp.volume = 0f;
        audioSourceTmp.Stop();
    }
    #endregion

    public void ResetBGMVolume()
    {
        StopAllCoroutines();
        var audioSource = bgmAudioSource;
        audioSource.volume = Mathf.Pow(10, dummyBGMVolume / 20f);
    }
}