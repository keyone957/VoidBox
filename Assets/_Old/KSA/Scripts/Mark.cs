using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : InteractionObject
{
    [SerializeField]
    private AudioClip clipMarkUp;
    [SerializeField] 
    private AudioClip clipMarkDown;
    [SerializeField]
    private float markUpDelayTime = 3;

    private AudioSource audioSource;
    private bool isPossibleHit = true;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public override void TakeDamage(int damage)
    {
        curHP -= damage;

        if (curHP <= 0 && isPossibleHit)
        {
            isPossibleHit = false;
            StartCoroutine("OnMarkDown");
        }
    }

    private IEnumerator onMarkDown()
    {
        audioSource.clip = clipMarkDown;
        audioSource.Play();

        yield return StartCoroutine(OnAnimation(0, 90));

        StartCoroutine("OnMarkUp");
    }

    private IEnumerator OnMarkUp()
    {
        audioSource.clip = clipMarkUp;
        audioSource.Play();

        yield return StartCoroutine(OnAnimation(90, 0));

        isPossibleHit = true;
    }

    private IEnumerator OnAnimation(float start, float end)
    {
        float percent = 0;
        float current = 0;
        float time = 1;

        while (percent < 1) 
        {
            current += Time.deltaTime;
            percent += current / time;

            transform.rotation = Quaternion.Slerp(Quaternion.Euler(start, 0, 0), Quaternion.Euler(end, 0, 0), percent);

            yield return null;
        }
    }
}
