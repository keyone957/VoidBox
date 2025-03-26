using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class JoyState : MonoBehaviour
{
    [SerializeField] Transform joyStickTransform;
    [SerializeField] private List<Texture> img;
    [SerializeField] private RawImage screenImg;
    private DiskRotater _diskRotater;
    private int currentImageIndex=0;
    private void Awake()
    {
        _diskRotater = FindObjectOfType<DiskRotater>();
    }
    //조이스틱의 x축 rotation에 따라서 디스크 index를 정해줌
    private void SetDiskIndex()
    {
        float rotationX = joyStickTransform.localRotation.x;
        if (rotationX <= -0.45f)
        {
            _diskRotater.diskIndex = 0;
        }
        else if (rotationX >= 0.45f)
        {
            _diskRotater.diskIndex = 2;
        }
        else
        {
            _diskRotater.diskIndex = 1;
        }
    }

    public void ChangeImg()
    {
        if ((int)DayOneManager.Instance.CurState < (int)DayOneManager.GameState.PUZZLE3) return;
        float rotationX = joyStickTransform.localRotation.x;
        if (rotationX>=0.35f&& currentImageIndex > 0)
        {
            currentImageIndex--;
            screenImg.texture = img[currentImageIndex];
        }
        else if (rotationX <= -0.35f&& currentImageIndex < img.Count - 1)
        {
            currentImageIndex++;
            screenImg.texture = img[currentImageIndex];
        }
    }

    //스틱 오브젝트에 넣어줄 이벤트
    //스틱을 잡고 놓은 후에 함수 실행
    public void UnSelectEvent()
    {
        if (DayOneManager.Instance.CurState == DayOneManager.GameState.PUZZLE2)
        {
            SetDiskIndex();
        }
    }
}