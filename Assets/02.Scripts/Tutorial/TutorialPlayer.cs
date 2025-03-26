using System.Collections;
using UnityEngine;

public class TutorialPlayer : MonoBehaviour
{
    [SerializeField] private Transform centerEye;
    [SerializeField] private TutorialDoor door;
    [SerializeField] private FadeScreen fadeScreen;
    private RaycastHit hit;
    private LayerMask layerMask = 1 << 10;

    private WaitForSeconds waitForLoad = new WaitForSeconds(4f);
    private void Start()
    {
        StartCoroutine(TrackingDoor());
    }
    private IEnumerator TrackingDoor()
    {
        yield return waitForLoad;

        while (door is null)
        {
            if (Physics.Raycast(centerEye.position, centerEye.forward, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.GetComponent<TutorialDoor>() is not null)
                {
                    door = hit.transform.GetComponent<TutorialDoor>();
                    Debug.Log("¹® ¹ß°ß");
                }
            }
            yield return null;
        }
        SoundManager.instance.StopSoundByClip("BangingOnDoor");

        var fadeDuration = fadeScreen.fadeDuration;
        fadeScreen.fadeDuration = 1f;
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        TutorialManager.Instance.PhaseUpdate();
        fadeScreen.fadeDuration = fadeDuration;
        fadeScreen.FadeIn();
    }
}
