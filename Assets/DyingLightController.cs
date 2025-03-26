using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingLightController : MonoBehaviour
{
    public Light flashLight;
    [SerializeField] private float maxIntensity;
    [SerializeField] private float minIntensity;
    [SerializeField] private float maxRange;
    [SerializeField] private float minRange;
    [SerializeField] private float flashDuration;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        flashLight.gameObject.SetActive(false);
        flashLight.intensity = maxIntensity;
        flashLight.range = maxRange;
        flashLight.innerSpotAngle = 0;
        flashLight.spotAngle = 179;
    }
    public void TriggerFlash()
    {
        StartCoroutine(FlashCoroutine());
    }
    public IEnumerator FlashCoroutine()
    {
        flashLight.gameObject.SetActive(true);
        float elapsedTime = 0f;

        while (elapsedTime < flashDuration)
        {
            float progress = 1 - (elapsedTime / flashDuration);

            flashLight.intensity = progress * (maxIntensity - minIntensity) + minIntensity;
            flashLight.range = progress * (maxRange - minRange) + minRange;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        flashLight.intensity = minIntensity;
        flashLight.range = minRange;
    }
}
