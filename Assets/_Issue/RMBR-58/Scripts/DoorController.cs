using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private bool isEnded = false; // 문 상태 (true = 닫힘, false = 열림)
    public GameObject[] doors; // 여러 개의 문 오브젝트
    [SerializeField] private Animator[] doorAnimators; // 각 문에 연결된 Animator 배열


    public void OpenDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            if (doorAnimators[i] != null)
            {
                doorAnimators[i].SetTrigger("OpenDoor"); // Animator의 OpenDoor 트리거 활성화
            }
            else
            {
                // Animator가 없으면 Transform 회전으로 열림 구현
                doors[i].transform.rotation = Quaternion.Euler(0, 90, 0); // Y축으로 90도 회전
            }
        }
    }

    public void CloseDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            if (doorAnimators[i] != null)
            {
                doorAnimators[i].SetTrigger("CloseDoor"); // Animator의 CloseDoor 트리거 활성화
            }
            else
            {
                // Animator가 없으면 Transform 회전으로 닫힘 구현
                doors[i].transform.rotation = Quaternion.Euler(0, 0, 0); // Y축 0도 회전 (닫힘)
            }
        }
    }
}
