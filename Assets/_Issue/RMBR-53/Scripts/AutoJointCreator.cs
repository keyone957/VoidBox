using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoJointCreator : MonoBehaviour
{
    public float connectionRadius = 1.0f; // 연결할 파편들의 최대 거리
    public bool useSpringJoint = false;   // Spring Joint 사용 여부
    public float springForce = 500f;      // 스프링 강도
    public float damper = 5f;             // 감쇠력

   


    void Start()
    {
        // 주변 파편들을 찾기 위해 OverlapSphere 사용
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
                        // Spring Joint 추가
                        SpringJoint joint = gameObject.AddComponent<SpringJoint>();
                        joint.connectedBody = rb;
                        joint.spring = springForce;
                        joint.damper = damper;
                    }
                    else
                    {
                        // Fixed Joint 추가
                        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                        joint.connectedBody = rb;
                    }
                }
            }
        }
    }
}
