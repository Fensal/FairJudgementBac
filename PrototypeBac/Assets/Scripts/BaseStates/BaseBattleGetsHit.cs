using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseBattleGetsHit : BaseBattleState
{
    protected float timer = 0.0f;

    public BaseBattleGetsHit(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Enter();
    }
    public override void Enter()
    {
        m_Character.GetComponent<Animator>().SetTrigger("Hit");

        m_Character.m_TotalForce.x = -0.2f * Math.Sign(m_Character.m_Renderer.transform.localScale.x);

        m_NextState = this;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (m_Character.m_TotalForce.x - m_Character.m_Deceleration > 0.0f)
        {
            m_Character.m_TotalForce.x -= m_Character.m_Deceleration;
        }
        else if (m_Character.m_TotalForce.x + m_Character.m_Deceleration < 0.0f)
        {
            m_Character.m_TotalForce.x += m_Character.m_Deceleration;
        }
        else
        {
            m_Character.m_TotalForce.x = 0.0f;
        }
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}