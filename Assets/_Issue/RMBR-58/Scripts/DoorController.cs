using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private bool isEnded = false; // �� ���� (true = ����, false = ����)
    public GameObject[] doors; // ���� ���� �� ������Ʈ
    [SerializeField] private Animator[] doorAnimators; // �� ���� ����� Animator �迭


    public void OpenDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            if (doorAnimators[i] != null)
            {
                doorAnimators[i].SetTrigger("OpenDoor"); // Animator�� OpenDoor Ʈ���� Ȱ��ȭ
            }
            else
            {
                // Animator�� ������ Transform ȸ������ ���� ����
                doors[i].transform.rotation = Quaternion.Euler(0, 90, 0); // Y������ 90�� ȸ��
            }
        }
    }

    public void CloseDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            if (doorAnimators[i] != null)
            {
                doorAnimators[i].SetTrigger("CloseDoor"); // Animator�� CloseDoor Ʈ���� Ȱ��ȭ
            }
            else
            {
                // Animator�� ������ Transform ȸ������ ���� ����
                doors[i].transform.rotation = Quaternion.Euler(0, 0, 0); // Y�� 0�� ȸ�� (����)
            }
        }
    }
}
