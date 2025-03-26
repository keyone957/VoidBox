using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerCountCheck : MonoBehaviour
{
    public bool submit { get; private set; }
    public bool answer { get; private set; }

    // ���� �亯 Ƚ�� üũ
    [SerializeField] private int cur_answer_count = 0;
    // ����Ǿ�� �ϴ� �� ����
    [SerializeField] private int submit_answer_num = 1;

    // ���� ���� ���� üũ
    [SerializeField] private int answer_count = 0;
    [SerializeField] AnswerSubmitBehavior answerSubmitBehavior;
    [SerializeField] private float delayTime = 0.5f; // answer check ���� ��ٸ� �ð�

    // �亯 ���� ����
    public void SubmitAnswer()
    {
        cur_answer_count++;
        UpdateAnswer();
    }

    // �亯 ���� ����
    public void DeleteAnswer()
    {
        cur_answer_count--;
        UpdateAnswer();
    }

    // ���� ���� ����
    public void RightAnswer()
    {
        answer_count++;
        UpdateAnswer();
    }

    // ���� ���� ����
    public void WrongAnswer()
    {
        answer_count--;
        UpdateAnswer();
    }


    private void UpdateAnswer()
    {
        submit = (cur_answer_count == submit_answer_num);
    }


    // ���� �� ���� ������ ȣ�� (ex. release �ÿ��� ȣ��)
    public void Submit()
    {
        StartCoroutine(SubmitWithDelay(delayTime));
    }

    private IEnumerator SubmitWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // ���� �ð� ���� �� submit �۾� ����
        if (submit)
        {
            answer = (answer_count == submit_answer_num);
            if (answerSubmitBehavior != null)
            {
                answerSubmitBehavior.AnswerSubmit(answer);
            }
        }
    }


}
