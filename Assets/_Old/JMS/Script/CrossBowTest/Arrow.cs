using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    public ArrowData arrowData;
    public ArrowData ArrowData { set { ArrowData = value; } }

    public void WatchMonsterInfo()
    {
        Debug.Log(arrowData.Name);
        Debug.Log(arrowData.Dmg);
        Debug.Log(arrowData.Att);
        Debug.Log(arrowData.Dot);
    }

    public void ChangeAtt(ArrowData newarrowData)
    {
        arrowData = newarrowData;
        arrowData.Level = 0;
        gameObject.GetComponent<Renderer>().material.color = newarrowData.aColor;
    }
}
