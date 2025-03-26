using System.Collections;
using UnityEngine;

public class SafeDial : MonoBehaviour
{
    public bool isGrabed;
    private Quaternion startRot;
    private float soundPlayIdx = 40f;
    private void Start()
    {
        startRot = this.transform.rotation;

        //그랩 시 발동되게 수정해야 함
        isGrabed = true;
        StartCoroutine(DialControllCoroutine()); 
    }
    public void UnSelect()
    {
        StopCoroutine(DialControllCoroutine());
        isGrabed = false;
    }
    /// <summary>
    /// 사운드 겹침 방지
    /// </summary>
    /// <returns></returns>
    private IEnumerator DialControllCoroutine()
    {
        while (isGrabed)
        {
            float dialRotY = Mathf.Abs((Quaternion.Inverse(this.transform.rotation) * startRot).eulerAngles.y);
            //시계방향
            if (dialRotY <= 180)
            {
                if (dialRotY >= soundPlayIdx)
                {
                    SoundManager.instance.PlaySound("Dial1", SoundType.SFX, 1.0f);
                    startRot = this.transform.rotation;
                    OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.Touch);

                    yield return new WaitForSeconds(0.1f);
                    OVRInput.SetControllerVibration(0.0f, 0.0f, OVRInput.Controller.Touch);
                }
            }
            //역방향
            else
            {
                if (Mathf.Abs((Quaternion.Inverse(startRot) * this.transform.rotation).eulerAngles.y) >= soundPlayIdx)
                {
                    SoundManager.instance.PlaySound("Dial1", SoundType.SFX, 1.0f);
                    startRot = this.transform.rotation;
                    OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.Touch);

                    yield return new WaitForSeconds(0.1f);
                    OVRInput.SetControllerVibration(0.0f, 0.0f, OVRInput.Controller.Touch);
                }
            }
            yield return null;
        }
    }
}
