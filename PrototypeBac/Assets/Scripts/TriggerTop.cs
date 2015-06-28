using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TriggerTop : MonoBehaviour
{
    BaseCharacterController m_Character;
    Transform m_TargetPosition;

    void Awake()
    {
        if (m_Character == null && Root.Instance != null)
        {
            m_Character = Root.Instance.m_Character;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        m_Character = GetComponentInParent<BaseCharacterController>();
        if (m_Character != null && other.gameObject.layer == LayerMask.NameToLayer("Climb"))
        {
            m_TargetPosition = other.transform.GetChild(0).transform;

            if (m_Character.m_CurrentState is MovementJump)
            {
                
                Vector3 newPosition = (m_Character.transform.position - transform.position) + other.transform.position;
                newPosition.z = m_Character.transform.position.z;
                m_Character.transform.position = newPosition;
                (m_Character.m_CurrentState as MovementJump).GrabLedgeTop();
            }

            if (m_Character.m_CurrentState.m_NextState is MovementGrabTop)
            {
                (m_Character.m_CurrentState.m_NextState as MovementGrabTop).m_TargetPosition = m_TargetPosition.position;
                (m_Character.m_CurrentState.m_NextState as MovementGrabTop).m_TargetPosition.z = m_Character.transform.position.z;
            }
            
        }
    }
}