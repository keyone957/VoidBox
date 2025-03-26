using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInven : MonoBehaviour
{
    [SerializeField] private List<WeaponBase> weapons = new List<WeaponBase>();

    private void Start()
    {
        SetWeapons();
    }
    public void SetWeapons()
    {
        weapons = WeaponCollection.instance.weapons;

    }
    public void ChangeWeapon(WeaponBase weapon)
    {
        if (weapon.transform.gameObject.activeInHierarchy) return;

        foreach (WeaponBase weaponBase in weapons)
        {
            if (weaponBase == weapon)
            {
                weaponBase.transform.gameObject.SetActive(true);
            }
            else weaponBase.transform.gameObject.SetActive(false);
        }
    }
    public void ChangeWeaponByWave(int wave)
    {
        //2스테이지 클리어 시 무기변경
        if (wave == 2)
        {
            int weaponIndex = wave - 1;
            if (weaponIndex < weapons.Count)
            {
                ChangeWeapon(weapons[weaponIndex]);
            }
        }
    }
}
