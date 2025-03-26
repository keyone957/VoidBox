using TMPro;
using UnityEngine;
using UnityEngine.UI;

//юс╫ц
public class WeaponChangeButton : MonoBehaviour
{
    public WeaponBase weapon;

    public TextMeshProUGUI textMeshPro;

    private void Start()
    {
        textMeshPro = transform.GetComponentInChildren<TextMeshProUGUI>();
        SetWeapon();
    }
    private void SetWeapon()
    {
        textMeshPro.text = weapon.weaponType.ToString();
    }
    public void ChangeWeapon(WeaponBase weapon)
    {
        if (weapon.transform.gameObject.activeInHierarchy) return;

        WeaponCollection.instance.SwitchWeapon(weapon);

        Debug.Log(WeaponCollection.instance.currentWeapon.name);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) ChangeWeapon(weapon);
    }
}
