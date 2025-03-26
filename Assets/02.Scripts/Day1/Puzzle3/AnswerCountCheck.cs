using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerCountCheck : MonoBehaviour
{
    public bool submit { get; private set; }
    public bool answer { get; private set; }

    // 현재 답변 횟수 체크
    [SerializeField] private int cur_answer_count = 0;
    // 제출되어야 하는 답 갯수
    [SerializeField] private int submit_answer_num = 1;

    // 현재 정답 갯수 체크
    [SerializeField] private int answer_count = 0;
    [SerializeField] AnswerSubmitBehavior answerSubmitBehavior;
    [SerializeField] private float delayTime = 0.5f; // answer check 까지 기다릴 시간

    // 답변 갯수 증가
    public void SubmitAnswer()
    {
        cur_answer_count++;
        UpdateAnswer();
    }

    // 답변 갯수 감소
    public void DeleteAnswer()
    {
        cur_answer_count--;
        UpdateAnswer();
    }

    // 정답 갯수 증가
    public void RightAnswer()
    {
        answer_count++;
        UpdateAnswer();
    }

    // 정답 갯수 감소
    public void WrongAnswer()
    {
        answer_count--;
        UpdateAnswer();
    }


    private void UpdateAnswer()
    {
        submit = (cur_answer_count == submit_answer_num);
    }


    // 제출 할 조건 만족시 호출 (ex. release 시에만 호출)
    public void Submit()
    {
        StartCoroutine(SubmitWithDelay(delayTime));
    }

    private IEnumerator SubmitWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 일정 시간 지연 후 submit 작업 수행
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
