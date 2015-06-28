using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseBattleLightAttackDown : BaseBattleState, IAttackState
{

    public BaseBattleLightAttackDown(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Enter();
    }
    public override void Enter()
    {
        m_NextState = this;
        m_Character.m_TotalForce.x = 0.0f;

        m_Character.m_WeaponCollider.enabled = true;
        m_Character.GetComponent<Animator>().SetTrigger("LightAttackDown");
        m_Character.GetComponent<Animator>().SetFloat("LookingRight", (float)Math.Sign(m_Character.m_Renderer.transform.localScale.x));
    }

    public override void Update()
    {
        base.Update();


    }

    public override IBaseState GetNextState()
    {
        if (m_NextState != this)
        {
            m_Character.m_WeaponCollider.collider.enabled = false;
        }

        return base.GetNextState();
    }

    public virtual void WeaponCollide(Collider other)
    {
    }

}

