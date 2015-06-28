using UnityEngine;
using System.Collections;

public class TriggerMiddle : MonoBehaviour
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
                (m_Character.m_CurrentState as MovementJump).GrabLedgeMiddle();
            }
        }
    }
}