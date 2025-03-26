using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using System;
using Knife.Effects.SimpleController;
public class WristInventoryUI : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();

    private Dictionary<int, GameObject> itemPairs = new Dictionary<int, GameObject>();
    [SerializeField] private InteractableUnityEventWrapper prevButton;
    [SerializeField] private InteractableUnityEventWrapper nextButton;

    private int currentItem;
    public bool haveItem { get { return items.Count > 0; } }
    void Awake()
    {
        itemPairs.Clear();
    }
#if UNITY_EDITOR
    private void OnGUI()
    {
        const int width = 100;
        const int height = 25;
        int x = 0;
        int y = 0;

        if (GUI.Button(new Rect(x, y, width, height), "prev Item"))
            ChangeItem(false);
        x += width;

        if (GUI.Button(new Rect(x, y, width, height), "Next Item"))
            ChangeItem(true);
        x += width;
    }
#endif
    private void Start()
    {
        prevButton.WhenUnselect.AddListener(() => ChangeItem(false));
        nextButton.WhenUnselect.AddListener(() => ChangeItem(true));
    }
    private void ChangeItem(bool isNext)
    {
        int currentItemNum = currentItem;
        int nextItemNum;

        if (!itemPairs[currentItemNum].activeSelf)
        {
            if (isNext)
            {
                nextItemNum = (currentItemNum + 1) % itemPairs.Count;
            }
            else
            {
                nextItemNum = (currentItemNum - 1 + itemPairs.Count) % itemPairs.Count;
            }

            itemPairs[nextItemNum].SetActive(true);
            currentItem = nextItemNum;
            Debug.Log($"아이템 활성화: {nextItemNum}");
            return;
        }

        if (isNext)
        {
            nextItemNum = (currentItemNum + 1) % itemPairs.Count;
            itemPairs[currentItemNum].SetActive(false);
        }
        else
        {
            nextItemNum = (currentItemNum - 1 + itemPairs.Count) % itemPairs.Count;
            itemPairs[currentItemNum].SetActive(false);
        }

        currentItem = nextItemNum;
    }

    public void RegistItem(Item item, WeaponBase weapon = null)
    {
        if (itemPairs.ContainsValue(item.gameObject)) return;
        int currentItemCount = items.Count;
        itemPairs.Add(currentItemCount, item.gameObject);
        items.Add(item);
        Debug.Log($"{currentItemCount} : {item.name}");
        currentItem = currentItemCount;
        item.itemNum = currentItemCount;

        if (weapon != null)
        {
            weapon.isGrabed = false;
        }
    }
    public void RemoveItem(int itemNum, WeaponBase weapon = null)
    {
        if (itemPairs[itemNum] is null) return;
        items.Remove(itemPairs[itemNum].GetComponent<Item>());
        itemPairs.Remove(itemNum);

        if (weapon != null)
        {
            var weaponPos = WeaponCollection.instance.NewWeaponPos;
            weapon.isGrabed = true;
            WeaponCollection.instance.SwitchWeapon(weapon);
            weapon.transform.SetParent(weaponPos);
            weapon.transform.position = weaponPos.position;
            weapon.transform.rotation = weaponPos.rotation;

        }
    }
    private void OnDisable()
    {
        prevButton.WhenUnselect.RemoveListener(() => ChangeItem(false));
        nextButton.WhenUnselect.RemoveListener(() => ChangeItem(true));
    }
}
