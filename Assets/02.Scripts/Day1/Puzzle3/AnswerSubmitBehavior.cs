using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AnswerSubmitBehavior : MonoBehaviour
{
    public UnityEvent Right;
    public UnityEvent Wrong;
    public void AnswerSubmit(bool answer)
    {
        if (answer)
        {
            RightAnswerSubmit();
        }

        else
        {
            WrongAnswerSubmit();
        }
    }

    void RightAnswerSubmit() {
        //Debug.Log("����");
        if (Right != null)
        {
            Right.Invoke();
            SoundManager.instance.PlaySound("ChairHandle", SoundType.SFX);
            StartCoroutine(RightAnswerCoroutine());
            
        }
    }
    void WrongAnswerSubmit() {
        //Debug.Log("����");
        if (Wrong != null)
        {
            Wrong.Invoke();
            SoundManager.instance.PlaySound("InCorrect", SoundType.SFX);
            DialogManager.instance.ShowDialog("D3"); // "이 순서가 아닌 것 같아. 번호를 잘 봐봐."
        }
    }

    private IEnumerator RightAnswerCoroutine()
    {
        DialogManager.instance.ShowDialog("D4"); // "잘했어!"

        yield return new WaitForSeconds(13f); // TODO hardcoded
        
        DayOneManager.Instance.NextState = DayOneManager.GameState.PUZZLECLEAR;
    }
}
