using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoor : MonoBehaviour
{
    private Floor floor;
    private void Start()
    {
        SoundManager.instance.PlaySound("BangingOnDoor", SoundType.SFX, true);
    }
    public void SpawnDoor(Vector3 spawnPoint)
    {
        this.transform.position = spawnPoint;
    }
}
