using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScriptSoundMatch : MonoBehaviour
{

    public bool hasSound;
    public float scriptLength = 3;
    public TextMeshProUGUI textMeshPro;

    float fadeDuration = 0.5f;

    public AudioSource voiceDub;
    public AudioClip voiceClip;
    public GameObject nextscript;

    public bool lastendcheck;
    

    void OnEnable()
    {
        if (hasSound)
        {
            if (!voiceDub.gameObject.activeInHierarchy)
            {
                voiceDub.gameObject.SetActive(true);
                
            }
            StartCoroutine(DubbingWithSound(voiceDub));
            FadeOut();
        }

        else
        {
            FadeOut();
            StartCoroutine(DubbingWithNoSound(scriptLength));
        }
    }


    IEnumerator DubbingWithSound(AudioSource voice)
    {
        //yield return new WaitForSeconds(0.6f);
        gameObject.GetComponent<AudioSource>().clip = voiceClip;

        gameObject.GetComponent<AudioSource>().Play();

        while (gameObject.GetComponent<AudioSource>().isPlaying)
        {
            yield return null;
        }
        FadeIn();
    }



    IEnumerator DubbingWithNoSound(float sec)
    {
        yield return new WaitForSeconds(sec);
        FadeIn();
    }



    public void FadeIn()
    {
        StartCoroutine(Fade(1, 0));
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(0, 1));
    }

    public IEnumerator Fade(float alphaIn, float alphaOut)
    {
        float timer = 0;
        
        while (timer <= fadeDuration)
        {
            float progress = timer / fadeDuration;
            textMeshPro.alpha = Mathf.Lerp(alphaIn, alphaOut, progress);

            timer += Time.deltaTime;
            yield return null;
        }

        if (textMeshPro.alpha < 0.1)
        {
            if (nextscript != null)
            {
                nextscript.SetActive(true);
            }
            else
            {
                lastendcheck = true; //just for PhoneCall end
            }
            gameObject.SetActive(false);
        }
        
    }

}