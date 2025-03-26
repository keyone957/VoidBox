using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] GameObject doorLock;
    [SerializeField] GameObject doorHandle;

    private void Update()
    {
        if (PuzzleManager.Instance.nowState == PuzzleManager.PuzzleState.OPENDOOR)
            OpenDoor();
    }
    public void OpenDoor()
    {
        doorLock.SetActive(false);
        doorHandle.GetComponent<OneGrabRotateTransformer>().Constraints.MinAngle.Value = -120;
    }
}
