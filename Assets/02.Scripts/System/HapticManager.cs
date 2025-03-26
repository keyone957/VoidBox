using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticManager : MonoBehaviour
{
    public static HapticManager instance;

    [SerializeField] private float duration = 0.1f;
    [SerializeField] private float intensity = 0.1f;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }
    
    void CreateHapticClip()
    {
        int sampleCount = Mathf.RoundToInt(duration * OVRHaptics.Config.SampleRateHz);

        OVRHapticsClip hapticClip = new OVRHapticsClip(sampleCount);

        for (int i = 0; i < sampleCount; i++)
        {
            float normalizedPosition = 1f - ((float)i / sampleCount);
            byte sample = (byte)(Mathf.Clamp01(intensity * normalizedPosition) * byte.MaxValue);

            hapticClip.WriteSample(sample);
        }

        OVRHaptics.RightChannel.Preempt(hapticClip);
    }
    public IEnumerator HapticTest()
    {
        bool testb = false;
        while (true)
        {
            if (!testb)
            {
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
                {
                    CreateHapticClip();
                    testb = true;
                    yield return new WaitForSeconds(1f);
                }
                yield return null;
            }
        }
    }
    public void StartHaptic(int interaction, float frequancy, int strength, OVRInput.Controller controller)
    {
        OVRHapticsClip clip = new OVRHapticsClip();
        for (int i = 0; i < interaction; i++)
        {
            clip.WriteSample(i % frequancy == 0 ? (byte)strength : (byte)0);
        }

        switch (controller)
        {
            case OVRInput.Controller.None:
                break;
            case OVRInput.Controller.LTouch:
                OVRHaptics.LeftChannel.Preempt(clip);
                break;
            case OVRInput.Controller.RTouch:
                OVRHaptics.RightChannel.Preempt(clip);
                break;
            case OVRInput.Controller.Touch:
                OVRHaptics.LeftChannel.Preempt(clip);
                OVRHaptics.RightChannel.Preempt(clip);
                break;
        }
    }
}
