using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorWall : MonoBehaviour
{
    [SerializeField] float openSpeed = 1.0f;
    [SerializeField] Transform wallOpenPosition = null;
    [SerializeField] GameObject Monitor;

    void Update()
    {
        if (DayOneManager.Instance.CurState == DayOneManager.GameState.PUZZLE2)
        {
            OpenWall();
            Monitor.SetActive(true);
        }
        else if(DayOneManager.Instance.CurState == DayOneManager.GameState.PUZZLE3)
        {
            openSpeed = -1;
            OpenWall();
            Monitor.SetActive(false);
        }
    }

    void OpenWall()
    {
        Transform transform = GetComponent<Transform>();

        if( openSpeed < 0) // Left Wall
        {
            if (transform.localPosition.x > 0)
                transform.Translate(0.05f * openSpeed, 0,0);
        }
        else                                // Right Wall
        {
            if (transform.localPosition.x < wallOpenPosition.localPosition.x)
                transform.Translate(0.05f * openSpeed, 0, 0);
        }
    }

    /// <summary>
    /// 움직이기 시작할 때, 멈춰야할 때 호출
    /// </summary>
    /// <param name="isMove"></param>
    private void MoveSound(bool isMove)
    {
        if (isMove) SoundManager.instance.PlaySound("WallOpen", SoundType.SFX, true);
        else SoundManager.instance.StopSoundByClip("WallOpen");
    } 
}
