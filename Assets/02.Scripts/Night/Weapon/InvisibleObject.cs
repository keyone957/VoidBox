using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleObject : MonoBehaviour
{
    [SerializeField] private float timer = 2.5f;
    
    private Coroutine coroutine;
    private void OnEnable()
    {
        if (this.gameObject.activeInHierarchy) coroutine = StartCoroutine(InvisibleRoutine(timer));
    }
    private void OnDisable()
    {
        StopCoroutine(coroutine);
    }
    private IEnumerator InvisibleRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        this.gameObject.SetActive(false);
    }
}
