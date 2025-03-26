using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WristWatch : MonoBehaviour
{
    [SerializeField] private VisualEffect visualEffect;
    public VisualEffect VisualEffect => visualEffect;
    [SerializeField] private WristWatchUI wristWatchUI;
    
    public bool isActive { get; set; }

    private void Start()
    {
        isActive = false;
        wristWatchUI.gameObject.SetActive(false);
        visualEffect.Stop();
    }
    public void OnHover()
    {
        bool isActive = !this.isActive;

        if (!isActive)
        {
            wristWatchUI.gameObject.SetActive(isActive);
            visualEffect.Stop();
        }
        else
        {
            wristWatchUI.gameObject.SetActive(isActive);
            wristWatchUI.OnUI();
            visualEffect.Play();
        }
        this.isActive = isActive; 
    }
}
