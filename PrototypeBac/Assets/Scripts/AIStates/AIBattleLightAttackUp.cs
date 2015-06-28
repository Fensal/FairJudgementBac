using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AIBattleLightAttackUp : BaseBattleLightAttackUp, IAttackState
{
    private float timer = 0.0f;

    private bool flag = true;

    public AIBattleLightAttackUp(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.LogWarning("[AI Battle State]: Light Attack Up");
        Enter();
    }
    public override void Enter()
    {
        base.Enter();

        FMOD_StudioSystem.instance.PlayOneShot("event:/Enemy/enemy_attack_light", m_Character.transform.position);

        m_EnemyController = (EnemyCharacterController)m_Character;
        m_OpponentController = (EnemyCharacterController)m_Character.m_Opponent;

        if (flag)
        {
            //m_EnemyController.updatePreviousActions(this.ToString());
            flag = false;
        }

        m_NextState = this;
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;


        if (timer > m_Character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + m_Character.m_AttackCooldown)
        {
            m_EnemyController.updatePreviousActions(this.ToString());
            m_NextState = new AIBattleIdle(m_Character);
        }

    }

    public override IBaseState GetNextState()
    {
        if (m_NextState != this)
        {
            m_EnemyController.updatePreviousActions(this.ToString());
            m_Character.m_WeaponCollider.enabled = false;
        }

        return base.GetNextState();
    }

    public override void WeaponCollide(Collider other)
    {
        BaseCharacterController EnemyController = other.gameObject.GetComponentInParent<BaseCharacterController>();

        if (m_NextState == this)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("CharacterWeapon"))
            {
                FMOD_StudioSystem.instance.PlayOneShot("event:/Weapons/weapon_impact", m_Character.transform.position);

                if (m_Character.m_Opponent.m_CurrentState is IAttackState)
                {
                    m_NextState = new AIBattleAttackDeflected(m_Character);
                }

                if (m_Character.m_Opponent.m_CurrentState is AIBattleParryUp)
                {
                    m_NextState = new AIBattleCounterAttacked(m_Character);
                    m_Character.m_Opponent.m_CurrentState.m_NextState = new AIBattleCounterAttackUp(m_Character.m_Opponent);
                }

                if (m_Character.m_Opponent.m_CurrentState is AIBattleBlockUp)
                {
                    //m_NextState = new AIBattleBlocked(m_Character);
                    m_Character.GetHit(5);
                    m_Character.m_Opponent.m_CurrentState.m_NextState = new AIBattleIdle(m_Character.m_Opponent);
                    m_Character.m_Opponent.GetComponent<Animator>().SetBool("BlockUp", false);
                    
                }
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("CharacterBody") && m_Character != EnemyController)
            {
                FMOD_StudioSystem.instance.PlayOneShot("event:/Weapons/weapon_impact", m_Character.transform.position);

                if (m_Character.m_Opponent.m_CurrentState is AIBattleParryUp)
                {
                    m_NextState = new AIBattleCounterAttacked(m_Character);
                    m_Character.m_Opponent.m_CurrentState.m_NextState = new AIBattleCounterAttackUp(m_Character.m_Opponent);
                }
                else if (m_Character.m_Opponent.m_CurrentState is AIBattleBlockUp)
                {
                    m_Character.GetHit(5);
                    //m_NextState = new AIBattleBlocked(m_Character);
                    m_Character.m_Opponent.m_CurrentState.m_NextState = new AIBattleIdle(m_Character.m_Opponent);
                    m_Character.m_Opponent.GetComponent<Animator>().SetBool("BlockUp", false);
                }

                else
                {
                    m_NextState = new AIBattleSuccessfulAttack(m_Character);
                    m_Character.m_Opponent.GetHit(5);
                }

            }
        }
    }
}

