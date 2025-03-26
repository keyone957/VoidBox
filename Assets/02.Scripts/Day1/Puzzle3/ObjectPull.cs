using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPull : MonoBehaviour
{
    // SmoothMoveObjectTarget ��ũ��Ʈ�� ������ Grabbable ������Ʈ�� ��ó���� Release �Ǿ������
    // ObjectPull ��ũ��Ʈ�� ���� empty object���� ���� ������������� �̵�
    public Transform baseTransform;

    private void OnEnable()
    {
        ObjectPullManager.RegisterObjectPull(transform);
    }

    private void OnDisable()
    {
        ObjectPullManager.UnregisterObjectPull(transform);
    }

}
