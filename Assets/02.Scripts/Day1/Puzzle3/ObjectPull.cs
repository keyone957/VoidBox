using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPull : MonoBehaviour
{
    // SmoothMoveObjectTarget 스크립트가 부착된 Grabbable 오브젝트가 근처에서 Release 되었을경우
    // ObjectPull 스크립트를 가진 empty object들중 가장 가까운지점으로 이동
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
