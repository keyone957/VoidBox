using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class KeyDetector : MonoBehaviour
{
    private TextMeshPro display;

    private KeyPadControll keyPadControll;
    // Start is called before the first frame update
    void Start()
    {
        display = GameObject.FindGameObjectWithTag("Display").GetComponentInChildren<TextMeshPro>();
        display.text = "";

        keyPadControll = GameObject.FindGameObjectWithTag("Keypad").GetComponent<KeyPadControll>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KeypadButton"))
        {
            var key = other.GetComponentInChildren<TextMeshPro>();
            if (key != null)
            {
                var keyFeedBack = other.gameObject.GetComponent<KeyFeedback>();

                if (key.text == "Back")
                {
                    if (display.text.Length > 0)
                        display.text = display.text.Substring(0, display.text.Length - 1);
                }
                else if (key.text == "Enter")
                {
                    var accessGranted = false;
                    if (display.text.Length > 0)
                    {
                        accessGranted = keyPadControll.CheckIfCorrect(Convert.ToInt32(display.text));
                    }

                    if(accessGranted == true)
                    {
                        display.text = "Open!";
                    }
                    else
                    {
                        display.text = "Retry";
                    }
                }
                else if (key.text == "Cancel")
                {
                    display.text = "";
                }
                else
                {
                    bool onlyNumbers = int.TryParse(display.text, out var value);
                    if(onlyNumbers == false)
                    {
                        display.text = "";
                    }

                    if (display.text.Length < 4)
                        display.text += key.text;
                }
                keyFeedBack.keyHit = true;
            }
        }
    }
}
