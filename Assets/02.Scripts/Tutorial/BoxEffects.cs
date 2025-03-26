using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxEffects : MonoBehaviour
{
    public AudioClip boxShake;

    void Shake()
    {
        AudioSource.PlayClipAtPoint(boxShake, transform.position);
    }
}
