using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class InventoryWeapon : MonoBehaviour
{
    [SerializeField] private WeaponBase weapon;
    [SerializeField] private WristInventoryUI inventoryUI;
    public void Start()
    {
        inventoryUI.RegistItem(this.GetComponent<Item>());
    }
    public void TakeWeapon()
    {
        StartCoroutine(TakeWeaponRoutine());
    }
    public IEnumerator TakeWeaponRoutine()
    {
        float timer = 0;

        while (timer <= 1.5f)
        {
            timer += Time.deltaTime;

            yield return null;
        }
        WeaponCollection.instance.GainNewWeapon(weapon);
        WeaponCollection.instance.SwitchWeapon(weapon);
        this.gameObject.SetActive(false);
    }

    public void RegistInventory()
    {

    }
}
