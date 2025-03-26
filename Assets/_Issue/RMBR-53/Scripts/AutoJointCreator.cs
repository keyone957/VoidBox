using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoJointCreator : MonoBehaviour
{
    public float connectionRadius = 1.0f; // ������ ������� �ִ� �Ÿ�
    public bool useSpringJoint = false;   // Spring Joint ��� ����
    public float springForce = 500f;      // ������ ����
    public float damper = 5f;             // �����

   


    void Start()
    {
        // �ֺ� ������� ã�� ���� OverlapSphere ���
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;

        Collider[] colliders = Physics.OverlapSphere(transform.position, connectionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.gameObject != gameObject)
            {
                Rigidbody rb = nearbyObject.attachedRigidbody;
                if (rb != null)
                {
                    if (useSpringJoint)
                    {
                        // Spring Joint �߰�
                        SpringJoint joint = gameObject.AddComponent<SpringJoint>();
                        joint.connectedBody = rb;
                        joint.spring = springForce;
                        joint.damper = damper;
                    }
                    else
                    {
                        // Fixed Joint �߰�
                        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                        joint.connectedBody = rb;
                    }
                }
            }
        }
    }
}
