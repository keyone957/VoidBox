using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordCountCheck : MonoBehaviour
{
    [SerializeField] AnswerCountCheck answerCountCheck;
    [SerializeField] GameObject answer;


    private void Start()
    {
        answerCountCheck = FindObjectOfType<AnswerCountCheck>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Answer")
        {  
            if (collision.gameObject == answer)
            {
                answerCountCheck.RightAnswer();
                SoundManager.instance.PlaySound("ObjectSetting", SoundType.SFX);
            }
            answerCountCheck.SubmitAnswer();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Answer")
        {
            if (collision.gameObject == answer)
            {
                answerCountCheck.WrongAnswer();
            }
            answerCountCheck.DeleteAnswer();
        }
    }
}
