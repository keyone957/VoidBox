using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInput : MonoBehaviour
{
    public float speed = 2f;

    public Rigidbody rb;

    [HideInInspector] public bool RotationEitherThumbstick = false;
    public OVRCameraRig CameraRig;
    public float RotationAngle = 45.0f;
    private Vector3 moveDir;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.back * speed * Time.deltaTime);


        if (Input.GetKey(KeyCode.Z))
            transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.C))
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        UpdateInput();
        Turn();
    }
    private void FixedUpdate()
    {
        Move();
    }
    void Turn()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickLeft) ||
            (RotationEitherThumbstick && OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft)))
        {
            transform.RotateAround(CameraRig.centerEyeAnchor.position, Vector3.up, -RotationAngle);
        }
        else if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickRight) ||
                 (RotationEitherThumbstick && OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight)))
        {
            transform.RotateAround(CameraRig.centerEyeAnchor.position, Vector3.up, RotationAngle);
        }
    }

    void UpdateInput()
    {
        Quaternion ort = CameraRig.centerEyeAnchor.rotation;
        Vector3 ortEuler = new Vector3(0f, ort.eulerAngles.y, 0f);
        ort = Quaternion.Euler(ortEuler);

        moveDir = Vector3.zero;
        Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        moveDir += ort * (primaryAxis.x * Vector3.right);
        moveDir += ort * (primaryAxis.y * Vector3.forward);
    }

    void Move()
    {
        rb.MovePosition(rb.position + moveDir * (speed * Time.fixedDeltaTime));
    }

    void RotatePlayerToHMD()
    {
        Transform root = CameraRig.trackingSpace;
        Transform centerEye = CameraRig.centerEyeAnchor;

        Vector3 prevPos = root.position;
        Quaternion prevRot = root.rotation;

        transform.rotation = Quaternion.Euler(0.0f, centerEye.rotation.eulerAngles.y, 0.0f);

        root.position = prevPos;
        root.rotation = prevRot;
    }
}