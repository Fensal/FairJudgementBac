using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BattleHeavyAttackUp : BaseBattleHeavyAttackUp, IAttackState
{
    private float timer = 0.0f;

    public BattleHeavyAttackUp(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Battle State]: Heavy Attack Up");
        Enter();
    }
    public override void Enter()
    {
        base.Enter();

        FMOD_StudioSystem.instance.PlayOneShot("event:/Darien/darien_attack_heavy", m_Character.transform.position);

        m_Character.GetComponent<Animator>().SetBool("AttackBlockedTest", false);

        m_NextState = this;

        m_PlayerController = (PlayerCharacterController)m_Character;

    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (timer > m_Character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + m_Character.m_AttackCooldown)
        {
            m_PlayerController.updatePreviousActions(this.ToString());
            m_NextState = new BattleIdle(m_Character);
        }
            
        
    }

    public override IBaseState GetNextState()
    {
        if (m_NextState != this)
        {
            m_Character.m_WeaponCollider.enabled = false;
        }

        return base.GetNextState();
    }

    public override void WeaponCollide(Collider other)
    {
        BaseCharacterController EnemyController = other.gameObject.GetComponentInParent<BaseCharacterController>();

        if (m_NextState == this)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("CharacterWeapon") && m_Character != other.transform.parent.parent.gameObject.GetComponent<BaseCharacterController>())
            {
                if (m_Character.m_Opponent.m_CurrentState is AIBattleParryUp)
                {
                    m_NextState = new BattleCounterAttacked(m_Character);
                    m_Character.m_Opponent.m_CurrentState.m_NextState = new AIBattleCounterAttackUp(m_Character.m_Opponent);
                }

                if (m_Character.m_Opponent.m_CurrentState is AIBattleBlockUp)
                {
                    m_NextState = new BattleBlocked(m_Character);
                }

                if (m_Character.m_Opponent.m_CurrentState is IAttackState)
                {
                    m_Character.m_CurrentState = new BattleAttackDeflected(m_Character);
                }
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("CharacterBody") && m_Character != EnemyController)
            {
                if (m_Character.m_Opponent.m_CurrentState is AIBattleParryUp)
                {
                    m_NextState = new BattleCounterAttacked(m_Character);
                    m_Character.m_Opponent.m_CurrentState.m_NextState = new AIBattleCounterAttackUp(m_Character.m_Opponent);
                }

                else if (m_Character.m_Opponent.m_CurrentState is AIBattleBlockUp)
                {
                    m_NextState = new BattleBlocked(m_Character);
                }
                else
                {
                    m_NextState = new BattleSuccessfulAttack(m_Character);
                    m_Character.m_Opponent.GetHit(10);
                }
                
            }
        }
    }
}
