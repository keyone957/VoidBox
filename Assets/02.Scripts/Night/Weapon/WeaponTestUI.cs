using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WeaponTestUI : MonoBehaviour
{
    public TextMeshProUGUI magazine;
    public TextMeshProUGUI maxMagazine;
    public TextMeshProUGUI currentWeapon;
    public TextMeshProUGUI damageText;

    [SerializeField] private List<WeaponTestUI> weapons;
    public void UIUpdate(int damage = 0)
    {
        magazine.text = "magazine : " + WeaponCollection.instance.currentWeapon._magazine.ToString();
        maxMagazine.text = "ammoMax : " + WeaponCollection.instance.currentWeapon._ammoMax.ToString();
        damageText.text = "damage : " + damage.ToString();
    }
    public void GetCurrentWeapon(WeaponBase weapon)
    {
        currentWeapon.text = weapon.weaponType.ToString();
    }
}   