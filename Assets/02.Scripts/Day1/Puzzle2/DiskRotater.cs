using System.Collections.Generic;
using UnityEngine;

public class DiskRotater : MonoBehaviour
{
    [SerializeField] private RectTransform[] Disks;
    [SerializeField] private float rotationSpeed = 100.0f;

    private bool[] isRotate = new bool[3];

    public List<AnswerBar> answerBars = new List<AnswerBar>();
    [SerializeField]  DiskPuzzleTimer diskPuzzleTimer;
    public int diskIndex;//디스크 Index

    void Awake()
    {
        InitDiskRotation();
    }

    void Update()
    {
        RotateDisk();             
    }
    
    public void OnPointerDown()
    {
        if (diskIndex == -1||DayOneManager.Instance.CurState!=DayOneManager.GameState.PUZZLE2) return;
        if (answerBars[diskIndex].isAnswer)
            answerBars[diskIndex].isAnswer = false;
        SoundManager.instance.PlaySound("Lever", SoundType.SFX);
        isRotate[diskIndex] = true;
    }

    public void OnPointerUp()
    {       
        if (diskIndex == -1||DayOneManager.Instance.CurState!=DayOneManager.GameState.PUZZLE2) return;
        isRotate[diskIndex] = false;
        SoundManager.instance.FadeOutSFX("Lever", 1.5f);
        CheckAnswer();
    }

    
    void InitDiskRotation()
    {
        //조이스틱을 건드리지 않으면 버튼 눌러도 실행 안되게
        diskIndex = -1;
        for (int i = 0; i < Disks.Length; i++)
        {
            isRotate[i] = false;
            //Disks[i].transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        }
    }

    void RotateDisk()
    {
        for (int i = 0; i < Disks.Length; i++)
        {
            if (isRotate[i] == true)
            {
                Disks[i].Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            }
        }
    }
    void CheckAnswer()
    {
        for(int i =0; i< 3; i++)
        {
            if (answerBars[i].isAnswer)
            {
                //Disks[i].rotation = Quaternion.Euler(0,0,0);
                Disks[i].rotation = Disks[0].rotation;  
            }
        }

        if (answerBars[1].isAnswer && answerBars[2].isAnswer)
            GameClear();
    }

    void GameClear()
    {
        diskPuzzleTimer.stopTimer = true;
        // Ÿ�̸� �ߴ� ���� Puzzle3�� �Ѿ
        DialogManager.instance.ShowDialog("C3_a_1"); // "해냈다."
        DayOneManager.Instance.NextState = DayOneManager.GameState.PUZZLE3;

        diskPuzzleTimer.ClearSoundPlay();
    }
}
