using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Player player;

    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI healthText;

    private GameObject uiObject;
    private void Awake()
    {
        uiObject = this.transform.parent.gameObject;
    }
    private void Start()
    {
        GlobalEvent.RegisterEvent(EventType.UpdateWeaponUI, UpdateWeaponUI);
    }
    public void ActiveUI(bool disable)
    {
        if (disable) uiObject.SetActive(false);
        else uiObject.SetActive(true);
    }
    public void SetWeaponUIPosition(Transform parent)
    {
        uiObject.transform.SetParent(parent.transform, false);
        uiObject.transform.localPosition = Vector3.zero;
        uiObject.transform.localRotation = Quaternion.identity;

    }
    private void UpdateWeaponUI()
    {
        //Debug.Log("무기 업데이트" +
        //    $"무기 {WeaponCollection.instance.currentWeapon._magazine} / {WeaponCollection.instance.currentWeapon._ammoMax}"
        //    + $"체력 {player.PlayerStatus.hp} / {player.PlayerStatus.maxHP}");
        ammoText.text = $"{WeaponCollection.instance.currentWeapon._magazine} / {WeaponCollection.instance.currentWeapon._ammoMax}";
        healthText.text = $"{player.PlayerStatus.hp} / {player.PlayerStatus.maxHP}";
    }
}
