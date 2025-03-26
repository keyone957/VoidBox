using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class CameraRendererSwitcher : MonoBehaviour
{
    public Camera camera;
    public UniversalRenderPipelineAsset urpAsset;
    public Image fadeImage;  // UI ĵ������ ��ü ȭ���� ���� �̹���
    public float fadeDuration = 0.5f;  // ���̵� ��/�ƿ��� �ɸ��� �ð�

    [SerializeField] bool situationActivated = false;

    void Update()
    {
        if (!situationActivated)
        {
            StartCoroutine(SmoothRendererSwitch(0));  // Default Renderer
        }
        else if (situationActivated)
        {
            StartCoroutine(SmoothRendererSwitch(1));  // Custom Renderer
        }
    }

    IEnumerator SmoothRendererSwitch(int rendererIndex)
    {
        // ���̵� �ƿ�
        yield return StartCoroutine(Fade(1.0f));

        // ������ ����
        SetRenderer(rendererIndex);

        // ���̵� ��
        yield return StartCoroutine(Fade(0.0f));
    }

    void SetRenderer(int rendererIndex)
    {
        var urp = GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset;
        if (urp != null && urp == urpAsset)
        {
            camera.GetUniversalAdditionalCameraData().SetRenderer(rendererIndex);
        }
    }

    IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, newAlpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, targetAlpha);
    }

    public void SetSituation(int index) {
        situationActivated = true;
    }

    public void EndSituation(int index) {
        situationActivated = false;
    }
}
