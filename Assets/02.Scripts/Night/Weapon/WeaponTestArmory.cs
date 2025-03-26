using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class WeaponTestArmory : MonoBehaviour
{
    public GameObject dummyWeapon;
    
    [SerializeField] private List<WeaponBase> weapons = new List<WeaponBase>();
    [SerializeField] private List<Button> buttons = new List<Button>();
    private Dictionary<WeaponBase, GameObject> weaponButtons = new Dictionary<WeaponBase, GameObject>();

    void Start()
    {
        SetWeapons();
    }

    public void SetWeapons()
    {
        weapons = WeaponCollection.instance.weapons;

        for (int i = 0; i < weapons.Count; i++)
        {
            buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = weapons[i].name;
            Debug.Log(buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
            int temp = i;
            buttons[temp].onClick.AddListener(() => ChangeWeapon(weapons[temp]));
        }
    }
    public void ChangeWeapon(WeaponBase weapon)
    {
        if (weapon.transform.gameObject.activeInHierarchy) return;

        WeaponCollection.instance.SwitchWeapon(weapon);
    }
}
