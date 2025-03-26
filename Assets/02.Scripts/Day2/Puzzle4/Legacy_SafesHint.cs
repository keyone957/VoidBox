using Oculus.VoiceSDK.UX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SafesHint : MonoBehaviour
{
    [SerializeField] private Safes safes;
    [SerializeField] private Image[] segment;
    private int passwordIdx {  get; set; }
    private bool filterMode {  get; set; }
    void Start()
    {
        passwordIdx = safes.password;
        passwordIdx = 0;
    }
    public IEnumerator FilterOn()
    {
        while (filterMode)
        {
            yield return null;
        }
    }
    public IEnumerator filterOff()
    {
        while (!filterMode)
        {
            yield return null;
        }
    }
}