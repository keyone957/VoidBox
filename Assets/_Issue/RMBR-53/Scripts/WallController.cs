using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public List<WallCutter> wallCutters;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 조건을 넣으면 각 벽에게 조건 부수는 신호가 전달
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
