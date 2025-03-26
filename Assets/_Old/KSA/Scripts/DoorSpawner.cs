using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject chip;
    [SerializeField] private GameObject memo;
    [SerializeField] private GameObject coffeeStick;
    private Vector3 spawnPos;
    private Vector3 m_spawnPos;
    private Vector3 objPos;
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private GameObject doorPreviewPrefab;
    [SerializeField] private LayerMask meshLayerMask;

    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    private OVRInput.Controller _activeController = OVRInput.Controller.RTouch;

    private GameObject _door;
    private GameObject _doorPreview;
    private bool _isPlaced;

    private (Vector3 point, Vector3 normal, bool hit) _leftHandHit;
    private (Vector3 point, Vector3 normal, bool hit) _rightHandHit;

    private void Start()
    {
        _doorPreview = Instantiate(doorPreviewPrefab, transform);
        _door = Instantiate(doorPrefab, transform);
        _door.SetActive(false);
    }

    void Spawn()
    {
        // FIXME: 오브젝트 위치 하드코딩됨. 수정필요
        memo.transform.position = spawnPos + new Vector3(0, 0.2f, 0);
        chip.transform.position = m_spawnPos + new Vector3(0, 0.2f, 0);
        Chip _chip = chip.transform.GetChild(0).GetComponent<Chip>();
        objPos += new Vector3(0, 0.2f, 0);
        coffeeStick.transform.position = objPos;
    }
    private void Update()
    {
        var togglePlacement = false;
        const OVRInput.Button buttonMask = OVRInput.Button.PrimaryIndexTrigger | OVRInput.Button.PrimaryHandTrigger;

        if (OVRInput.GetDown(buttonMask, OVRInput.Controller.LTouch))
        {
            _activeController = OVRInput.Controller.LTouch;
            togglePlacement = true;
        }
        else if (OVRInput.GetDown(buttonMask, OVRInput.Controller.RTouch))
        {
            _activeController = OVRInput.Controller.RTouch;
            togglePlacement = true;
        }

        var leftRay = new Ray(leftHand.position, leftHand.forward);
        var rightRay = new Ray(rightHand.position, rightHand.forward);

        var leftRaySuccess = Physics.Raycast(leftRay, out var leftHit, 100.0f, meshLayerMask);
        var rightRaySuccess = Physics.Raycast(rightRay, out var rightHit, 100.0f, meshLayerMask);

        _leftHandHit = (leftHit.point, leftHit.normal, leftRaySuccess);
        _rightHandHit = (rightHit.point, rightHit.normal, rightRaySuccess);
        var active = _activeController == OVRInput.Controller.LTouch ? _leftHandHit : _rightHandHit;

        if (togglePlacement && active.hit) TogglePlacement(active.point, active.normal);

        if (!_isPlaced && active.hit)
        {
            // update the position of the preview to match the raycast.
            var doorPreviewTransform = _doorPreview.transform;

            doorPreviewTransform.position = new Vector3(active.point.x, active.point.y, active.point.z);
            doorPreviewTransform.up = Vector3.up;
            doorPreviewTransform.rotation = Quaternion.LookRotation(active.normal);
        }
    }

    private void TogglePlacement(Vector3 point, Vector3 normal)
    {
        if (_isPlaced)
        {
//            _door.SetActive(false);
//            _doorPreview.SetActive(true);

//           _isPlaced = false;
        }
        else
        {
            var doorTransform = _door.transform;
            // ���鿡 �ٿ��� ����
            //doorTransform.position = new Vector3(point.x, 1, point.z);

            // ���� �پ������� grab�� �Ұ����ϹǷ� ���� ���⿡ �����ϵ� ����
            doorTransform.position = new Vector3(rightHand.position.x, 2f, rightHand.position.z);
            doorTransform.up = Vector3.up;
            doorTransform.rotation = Quaternion.LookRotation(normal) * Quaternion.Euler(0, 180f, 0);

            _door.SetActive(true);
            _doorPreview.SetActive(false);
            chip.SetActive(true);
            memo.SetActive(true);
            
            spawnPos = GameObject.Find("ChipSpawnPos").transform.position;
            m_spawnPos = GameObject.Find("MemoSpawnPos").transform.position;
            objPos = GameObject.Find("ObjPos").transform.position;

            Spawn();
            _isPlaced = true;
        }
    }
}
