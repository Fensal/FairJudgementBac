using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseBattleHeavyAttackUp : BaseBattleState, IAttackState
{

    public BaseBattleHeavyAttackUp(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Enter();
    }
    public override void Enter()
    {
        m_NextState = this;

        m_Character.m_TotalForce = Vector3.zero;

        m_Character.m_WeaponCollider.enabled = true;
        m_Character.GetComponent<Animator>().SetTrigger("HeavyAttackUp");
        m_Character.GetComponent<Animator>().SetFloat("LookingRight", (float)Math.Sign(m_Character.m_Renderer.transform.localScale.x));
    }

    public override void Update()
    {
        base.Update();
            
        
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }


    public virtual void WeaponCollide(Collider other)
    {
        //BaseCharacterController EnemyController = other.gameObject.GetComponentInParent<BaseCharacterController>();

        //if (other.gameObject.layer == 1 << LayerMask.NameToLayer("CharacterBody") && m_Character != EnemyController)
        //{
        //    //TODO: Deal Damage
        //    m_NextState = new BattleSuccessfulAttack(m_Character);
        //}
        //if (other.gameObject.layer == 1 << LayerMask.NameToLayer("CharacterWeapon") && EnemyController != null)
        //{
        //    if (EnemyController.m_CurrentState is BattleParry)
        //    {
        //        m_NextState = new BattleCounterAttacked(m_Character);
        //    }

        //    if (EnemyController.m_CurrentState is BattleBlockUp)
        //    {
        //        m_NextState = new BattleBlocked(m_Character);
        //    }
        //}
    }
}
