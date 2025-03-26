using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArrowData", menuName = "ArrowScriptable/CreateArrowData", order = int.MaxValue)]
public class ArrowData : ScriptableObject
{
    [SerializeField]
    private float dmg;
    public float Dmg { get { return dmg; } set { dmg = value; } }

    [SerializeField]
    private float dot;
    public float Dot { get { return dot; } set { dot = value; } }

    [SerializeField]
    private string arrowName;
    public string Name { get { return arrowName; } set { arrowName = value; } }

    [SerializeField]
    private int level;
    public int Level { get { return level; } set { level = value%4; } }

    [SerializeField]
    private Color color;
    public Color aColor { get { return color; } set { color = value; } }

    [SerializeField]
    private int att;
    public int Att { get { return att; } set { att = value; } }
}