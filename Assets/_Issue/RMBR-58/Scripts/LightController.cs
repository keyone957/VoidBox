using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    public GameObject[] spotlights; // ���� ���� ���̶���Ʈ �迭
    public float delayBetweenLights = 0.5f; // ����Ʈ�� �� ���� ����
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
                spotlights[currentLightIndex].SetActive(false); // ���� ����Ʈ ����
            }

            currentLightIndex++;
            yield return new WaitForSeconds(delayBetweenLights); // ���� �ð� ���
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
                spotlights[currentLightIndex].SetActive(true); // ���� ����Ʈ ����
            }

            currentLightIndex--;
            yield return new WaitForSeconds(delayBetweenLights); // ���� �ð� ���
        }
    }
}
