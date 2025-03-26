using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordCheck : MonoBehaviour
{
    [SerializeField] AnswerCheck answerCheck;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Answer")
        {
            answerCheck.RightAnswer();
            SoundManager.instance.PlaySound("Dial2", SoundType.SFX);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Answer")
        {
            answerCheck.WrongAnswer();
            SoundManager.instance.PlaySound("Dial2", SoundType.SFX);
        }
    }
}
