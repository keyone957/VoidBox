using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle3Manager : MonoBehaviour
{
   private static Puzzle3Manager _instance;
   public static Puzzle3Manager Instance
   {
      get
      {
         if (_instance == null)
         {
            _instance = FindObjectOfType<Puzzle3Manager>();
            if (_instance == null)
            {
               GameObject singletonObject = new GameObject(nameof(Puzzle3Manager));
               _instance = singletonObject.AddComponent<Puzzle3Manager>();
            }
         }
         return _instance;
      }
   }

   [SerializeField] private List<ResetObjectPos> items;
   // 현재 답변 횟수 체크
   [SerializeField] public int curSubmitCnt = 0;
   [SerializeField] public int answer_count = 0;
   private int submitAnswerCnt = 5;
   [SerializeField] public SmoothMoveObject chairObj;
  
   void Awake()
   {
      if (_instance != null && _instance != this)
      {
         Destroy(gameObject);
         return;
      }

      _instance = this;
   }
   public void CheckAnswer()
   {
      if (curSubmitCnt == submitAnswerCnt)
      {
         if (answer_count == 5)
         {
            SoundManager.instance.PlaySound("ChairHandle", SoundType.SFX);
            chairObj.ObjectRelease();
            StartCoroutine(RightAnswerCoroutine());
         }
         else
         {
            DialogManager.instance.ShowDialog("D3");
            Debug.Log("퍼즐 오답입니다.");
            WrongAnswer();
            ResetCounts();
         }
      }
   }
   public void RightAnswer()
   {
      answer_count++;
      CheckAnswer();
   }

   private void ResetCounts()
   {
      curSubmitCnt = 0;
      answer_count = 0;
   }

   public void WrongAnswer()
   {
      foreach (ResetObjectPos resetPos in items)
      {
         resetPos.ObjectToTarget();
      }
   }
   
   private IEnumerator RightAnswerCoroutine()
   {
      DialogManager.instance.ShowDialog("D4"); // "잘했어!"

      yield return new WaitForSeconds(13f); // TODO hardcoded
        
      DayOneManager.Instance.NextState = DayOneManager.GameState.PUZZLECLEAR;
   }
}
