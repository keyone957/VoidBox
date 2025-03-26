using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class WristBand : MonoBehaviour
{
    [SerializeField] private WristWatchUI wristBandUI;
    private InteractableUnityEventWrapper eventWrapper;
    private void Awake()
    {
        eventWrapper = GetComponent<InteractableUnityEventWrapper>();
        eventWrapper.WhenHover.AddListener(OpenBandUI);
    }
    private void OnEnable()
    {
        StartCoroutine(CheckWristRotation());
    }
    private void OpenBandUI()
    {
        wristBandUI.gameObject.SetActive(true);
    }
    private IEnumerator CheckWristRotation()
    {
        bool isActive = true;
        Quaternion startRotation = transform.rotation;
        while (isActive)
        {
            float rot1 = Mathf.Abs((Quaternion.Inverse(this.transform.rotation) * startRotation).eulerAngles.y);
            float rot2 = Mathf.Abs((Quaternion.Inverse(startRotation) * this.transform.rotation).eulerAngles.y);
            if (rot1 >= 80 || rot2 >= 80)
            {
                isActive = false;
            }
            yield return null;
        }
        //wristBandUI.ActiveBandPanel(false);
    }
}
