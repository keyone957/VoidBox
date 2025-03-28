using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FadeScreen : MonoBehaviour
{
    public bool fadeOnStart = true;
    public float fadeDuration = 2;
    public Color fadeColor;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color", fadeColor);
        if (fadeOnStart)
            FadeIn();
    }

    void Update() // TODO 여기가 아니라 뭔가 다른 곳에서 체크하고 싶음.
    {
        if (DayOneManager.Instance is not null) 
            if (DayOneManager.Instance.CurState == DayOneManager.GameState.GAMECLEAR) FadeOut();
    }

    public void FadeIn() => Fade(1, 0);
    public void FadeOut() => Fade(0, 1);
    
    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut)); 
    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0;
        while(timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer/fadeDuration);
            rend.material.SetColor("_Color",newColor);

            timer += Time.deltaTime;
            yield return null;  
        }

        Color newColor2 = fadeColor;
        newColor2.a = alphaOut;
        rend.material.SetColor("_Color", newColor2);
    }
}
