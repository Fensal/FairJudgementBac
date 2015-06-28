using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseBattleHeavyAttackDown : BaseBattleState, IAttackState
{

    public BaseBattleHeavyAttackDown(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Enter();
    }
    public override void Enter()
    {
        m_NextState = this;

        m_Character.m_TotalForce = Vector3.zero;

        m_Character.m_WeaponCollider.enabled = true;
        m_Character.GetComponent<Animator>().SetTrigger("HeavyAttackDown");
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
    }
}
