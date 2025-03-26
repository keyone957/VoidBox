using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class OnHitEffectController : MonoBehaviour
{
    [SerializeField] private Image bloodEffectImage;

    public Volume OnHitVolume; // Global Volume ������Ʈ ����
    private Vignette vignette; // Vignette ȿ��
    
    private bool isHit = false;
    public float fadeSpeed = 1.0f; // ���Ʈ�� ������� �ӵ�
    public Color hitColor = Color.red; // �ǰ� �� ���Ʈ ����

    private Color startColor;
    private float startAlpha = 0.1f;
    private float duration = 1f;

    void Start()
    {
        startColor = bloodEffectImage.color;
        startColor.a = startAlpha;
        bloodEffectImage.color = startColor;
        bloodEffectImage.enabled = false;

        if (OnHitVolume.profile.TryGet(out vignette))
        {
            // �ʱ� ���� ���� (������ �ʰ�)
            vignette.intensity.value = 0f;
            vignette.color.value = hitColor;
        }
        GlobalEvent.RegisterEvent(EventType.OnPlayerHitEffect, StartBloodEffect);
    }

    void Update()
    {
        // ����׿� �ڵ�: ���콺 ���� ��ư Ŭ�� �� �ǰ� ȿ�� Ʈ����
        if (Input.GetMouseButtonDown(0))
        {
            StartBloodEffect();
        }

        // ���Ʈ ���� ���� ó��
        if (isHit)
        {
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0f, fadeSpeed * Time.deltaTime);

            if (vignette.intensity.value <= 0.01f)
            {
                vignette.intensity.value = 0f;
                isHit = false;
            }
        }
    }
    private void StartBloodEffect()
    {
        StartCoroutine(HitEffectRoutine());
    }
    private IEnumerator HitEffectRoutine()
    {
        float alpha = bloodEffectImage.color.a;
        float startAlpha = alpha;
        float timer = 0;
        Color color = bloodEffectImage.color;
        bloodEffectImage.enabled = true;
        while (timer <= duration)
        {
            timer += Time.deltaTime;
            alpha = Mathf.Lerp(startAlpha, 0f, timer / duration);
            color.a = alpha;
            bloodEffectImage.color = color;
            yield return null;
        }
        Debug.Log("End Effect");
        bloodEffectImage.enabled = false;
        bloodEffectImage.color = startColor;
    }
    public void OnHit()
    {
        // �ǰ� �� ������ ���� ���Ʈ ȿ�� Ȱ��ȭ
        if (vignette != null)
        {
            vignette.intensity.value = 0.5f; // ���Ʈ ���� ���� ����
            isHit = true;
        }
    }
}
