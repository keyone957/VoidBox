using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockCheck : MonoBehaviour
{
    [SerializeField] private List<SafeDial> safeDials = new List<SafeDial>(); 

    [SerializeField] GameObject locked_script;
    [SerializeField] AnswerCheck answercheck;

    [SerializeField] OneGrabRotateTransformer transformer;


    // Update is called once per frame
    public void TryToOpen()
    {
        //if (DayOneManager.Instance.CurState != DayOneManager.GameState.PUZZLE1) return;

        if (answercheck.answer)
        {
            transformer.Constraints.MinAngle.Constrain = false;
            DayOneManager.Instance.NextState = DayOneManager.GameState.PUZZLE2;

            foreach (SafeDial dial in safeDials)
            {
                dial.UnSelect();
            }

            SoundManager.instance.PlaySound("SafeOpen2", SoundType.SFX);
        }

        if (!answercheck.answer)
        {
            if (transformer != null)
            {
                transformer.Constraints.MinAngle.Constrain = true;
            }
            // if (!locked_script.activeSelf)
            // {
            //     locked_script.SetActive(true);
            // }
            SoundManager.instance.PlaySound("SafeLocked", SoundType.SFX);
            
            DialogManager.instance.ShowDialog("B2"); // "열리지 않아... 다시 생각해봐"
        }
    }
}
