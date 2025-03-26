using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnswerBar : MonoBehaviour
{
    public bool isAnswer = false;

    private void Start()
    {
        isAnswer = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        isAnswer = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    { 
        isAnswer = false;
    }
}
