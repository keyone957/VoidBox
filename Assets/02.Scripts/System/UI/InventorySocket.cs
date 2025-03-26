using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
public class InventorySocket : MonoBehaviour
{
    [SerializeField] private WristInventoryUI inventoryUI;
    public GameObject item { get; private set; }

    public void RegisterItem(Item item)
    {
        this.item = item.gameObject;
        inventoryUI.RegistItem(item);
        Debug.Log($"RegistItem {item.name}");
    }
    public void RemoveItem(int item)
    {
        Debug.Log($"RemoveItem {item}");
        this.item = null;
        inventoryUI.RemoveItem(item);
    }
}
