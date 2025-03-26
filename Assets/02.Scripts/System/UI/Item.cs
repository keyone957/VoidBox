using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class Item : MonoBehaviour
{
    [SerializeField] private InteractableUnityEventWrapper eventWrapper;
    [SerializeField] private SmoothMoveObject smoothObj;
    private InventorySocket currentSocket;
    private bool inSocket;
    private bool OnUpdateScaled;

    private Quaternion snapPose = new Quaternion(180, 90, 90, 1);
    private Vector3 startScale;
    [SerializeField] private float scaleOffset;
    public int itemNum { get; set; }
    void Start()
    {
        eventWrapper.WhenUnselect.AddListener(UnSelect);
        startScale = transform.localScale;
    }
    private void UnSelect()
    {
        if (inSocket)
        {
            transform.SetParent(currentSocket.transform);
            StartCoroutine(smoothObj.ObjectMove());
            transform.rotation = snapPose;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InventorySocket>() is not null)
        {
            Debug.Log("HaveSnap");
            smoothObj.obj_base = other.transform;
            inSocket = true;
            currentSocket = other.GetComponent<InventorySocket>();
            currentSocket.RegisterItem(this);
        }
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.GetComponent<InventorySocket>() is not null && inSocket)
    //    {
    //        inSocket = false;
    //        transform.SetParent(null);
    //        currentSocket.RemoveItem(itemNum);
    //        itemNum = -1;
    //    }
    //}
}
