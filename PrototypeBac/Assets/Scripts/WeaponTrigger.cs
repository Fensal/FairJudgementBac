using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{
    public BaseCharacterController m_Character;

    void Awake()
    {
        if (m_Character == null && Root.Instance != null)
        {
            m_Character = Root.Instance.m_Character;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (m_Character != null && m_Character.m_CurrentState is IAttackState)
        {
            (m_Character.m_CurrentState as IAttackState).WeaponCollide(other);
        }
    }
}