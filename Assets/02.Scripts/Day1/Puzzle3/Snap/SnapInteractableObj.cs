using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Oculus.Interaction;
using UnityEngine;

public class SnapInteractableObj : MonoBehaviour
{
    [SerializeField] private GameObject answerObj;
    private SnapInteractable snapInteractalbe;
    private bool isCorrectAnswer = false; 

    private void Start()
    {
        snapInteractalbe = GetComponent<SnapInteractable>();
    }

    public void SelectInteractor()
    {
        GameObject interactorObj = (snapInteractalbe.SelectingInteractorViews.First().Data as MonoBehaviour).gameObject;
        Puzzle3Manager.Instance.curSubmitCnt++;
       
        isCorrectAnswer = (interactorObj.transform.parent.gameObject == answerObj);
       
        if (isCorrectAnswer)
        {
            SoundManager.instance.PlaySound("ObjectSetting", SoundType.SFX);
            Puzzle3Manager.Instance.RightAnswer();
        }
        else
        {
            Puzzle3Manager.Instance.CheckAnswer();
        }
    }

    public void UnSelectInteractor()
    {
        Puzzle3Manager.Instance.curSubmitCnt--;
        if (isCorrectAnswer)
        {
            Puzzle3Manager.Instance.answer_count--;
        }
    }
}