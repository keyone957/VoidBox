using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHeadSet : MonoBehaviour
{
    private IEnumerator OnSelectCoroutine()
    {
        SoundManager.instance.PlaySound("InventoryPutIn", SoundType.SFX);

        yield return new WaitForSeconds(1.0f);
        Debug.Log("VR Select ++ 1f");
        SoundManager.instance.StopSoundByType(SoundType.BGM);
        SoundManager.instance.PlaySound("Siren", SoundType.SFX, true);
        SoundManager.instance.PlaySound("Day1_BGM2", SoundType.BGM, true);
        this.gameObject.SetActive(false);

    }
    public void OnUnselected()
    {
        // DialogManager.instance.ShowDialog("B3"); // "방금 너가 챙긴 게 개조 VR 헤드셋이야 ~" TODO match timing with B4
        StartCoroutine(OnSelectCoroutine());
    }
}
