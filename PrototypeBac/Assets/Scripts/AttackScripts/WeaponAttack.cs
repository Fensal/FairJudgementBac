using UnityEngine;
using System.Collections;

public class WeaponAttack : MonoBehaviour {
    [SerializeField]
    public BaseCharacterController m_Character;

    void OnTriggerEnter(Collider other)
    {
        if (m_Character.m_CurrentState is IAttackState)
        {
            (m_Character.m_CurrentState as IAttackState).WeaponCollide(other);
        }
    }
}
