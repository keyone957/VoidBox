using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public List<WallCutter> wallCutters;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // ������ ������ �� ������ ���� �μ��� ��ȣ�� ����
        {
            CollapseAllWalls();
        }
    }

    public void CollapseAllWalls()
    {
        foreach (WallCutter cutter in wallCutters)
        {
            if (cutter != null)
            {
                cutter.FragmentWall();
                cutter.CollapseWall();
            }
        }
    }
}
