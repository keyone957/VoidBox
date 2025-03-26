using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SevenSegment : MonoBehaviour
{
    [SerializeField] private Image[] hundredsImages;
    [SerializeField] private Image[] tensImages;
    [SerializeField] private Image[] onesImages;

    [SerializeField] private int segmentIdx;
    [SerializeField] private Safes safes;

    private int ones, tens, hundreds;

    //   -- 0 --
    //  |       |
    //  5       1
    //  |       |
    //   -- 6 --
    //  |       |
    //  4       2
    //  |       |
    //   -- 3 --

    private bool[,] segmentIsActive = new bool[,] {

        {true,  true,  true,  true,  true,  true,  false}, // 0
        {false, true,  true,  false, false, false, false}, // 1
        {true,  true,  false, true,  true,  false, true }, // 2
        {true,  true,  true,  true,  false, false, true }, // 3
        {false, true,  true,  false, false, true,  true }, // 4
        {true,  false, true,  true,  false, true,  true }, // 5
        {true,  false, true,  true,  true,  true,  true }, // 6
        {true,  true,  true,  false, false, false, false}, // 7
        {true,  true,  true,  true,  true,  true,  true }, // 8
        {true,  true,  true,  true,  false, true,  true }  // 9
    };

    private void Start()
    {
        StartCoroutine(SetSegmentNum(safes.GetSafeIndex(safes.safe)));
    }
    public IEnumerator SetSegmentNum(int segmentIdx)
    {
        yield return new WaitForSeconds(1f);

        SplitNumber(segmentIdx);
        yield return new WaitForSeconds(1f);
        ShowNumber(onesImages, ones);
        ShowNumber(tensImages, tens);
        ShowNumber(hundredsImages, hundreds);
    }
    private void SplitNumber(int segmentIdx)
    {
        ones = segmentIdx % 10;
        tens = (segmentIdx / 10) % 10;
        hundreds = (segmentIdx / 100) % 10;
    }
    // 특정 숫자를 표시하는 함수
    private void ShowNumber(Image[] objs, int number)
    {
        if (number < 0 || number > 9)
        {
            return;
        }

        for (int i = 0; i < objs.Length; i++)
        {
            // 각 세그먼트를 활성화/비활성화
            objs[i].enabled = segmentIsActive[number, i];
        }
    }
}
