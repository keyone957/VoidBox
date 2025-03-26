using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPadDoor : MonoBehaviour
{
    [SerializeField] GameObject locked_script;
    [SerializeField] KeyPadControll keypadanswercheck;

    [SerializeField] OneGrabRotateTransformer transformer;


    // Update is called once per frame
    public void TryToOpen()
    {
        if (keypadanswercheck.answer)
        {
            transformer.Constraints.MinAngle.Constrain = false;
            Debug.Log("열기시도");
        }

        if (!keypadanswercheck.answer)
        {
            if (transformer != null)
            {
                transformer.Constraints.MinAngle.Constrain = true;
    
            }
            if (!locked_script.activeSelf)
            {
                locked_script.SetActive(true);
            }
        }
    }
}
