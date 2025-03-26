using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowUpgrader : MonoBehaviour
{
    [SerializeField] private ArrowData arrowData;

    private void OnTriggerEnter(Collider other)
    {
        ChangeArrow(other.gameObject.GetComponent<Arrow>());
      
    }

    private void ChangeArrow(Arrow arrow)
    {
        if (arrow.arrowData != arrowData)
        {
            arrow.ChangeAtt(arrowData);
        }
        else
            arrow.arrowData.Level++;
    }
}
