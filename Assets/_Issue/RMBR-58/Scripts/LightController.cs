using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    public GameObject[] spotlights; // 여러 개의 스팟라이트 배열
    public float delayBetweenLights = 0.5f; // 라이트를 끌 때의 간격
    private int currentLightIndex = 0;

    public void StartTurningOffLights()
    {
        currentLightIndex = 0;
        StartCoroutine(TurnOffLightsSequentially());
    }

    private IEnumerator TurnOffLightsSequentially()
    {
        Debug.Log("Light turnOff");
        while (currentLightIndex < spotlights.Length)
        {
            if (spotlights[currentLightIndex] != null)
            {
                spotlights[currentLightIndex].SetActive(false); // 현재 라이트 끄기
            }

            currentLightIndex++;
            yield return new WaitForSeconds(delayBetweenLights); // 지연 시간 대기
        }
    }

    public void StartTurningOnLights()
    {
        currentLightIndex = spotlights.Length - 1;
        StartCoroutine(TurnOnLightsSequentially());
    }

    private IEnumerator TurnOnLightsSequentially()
    {
        Debug.Log("Light turnOn");

        while (currentLightIndex >= 0)
        {
            if (spotlights[currentLightIndex] != null)
            {
                spotlights[currentLightIndex].SetActive(true); // 현재 라이트 끄기
            }

            currentLightIndex--;
            yield return new WaitForSeconds(delayBetweenLights); // 지연 시간 대기
        }
    }
}
