using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerCheck : MonoBehaviour
{
    public bool answer { get; private set; }
    [SerializeField] private int answer_count = 0;
    [SerializeField] private int answer_num = 1;

    public void RightAnswer()
    {
        answer_count++;
        UpdateAnswer();
    }

    public void WrongAnswer()
    {
        answer_count--;
        UpdateAnswer();
    }

    private void UpdateAnswer()
    {
        answer = (answer_count == answer_num);

        if (answer) SoundManager.instance.PlaySound("SafeOpen", SoundType.SFX);
    }
}
