using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseBattleDash : BaseBattleState
{
    protected int m_Ortientation;

    public BaseBattleDash(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Enter();
    }
    public override void Enter()
    {
        m_NextState = this;

        if (m_Character.m_FacingRight)
        {
            m_Ortientation = 1;
        }
        else
        {
            m_Ortientation = -1;
        }

        m_Character.m_TotalForce.x = 0.3f * m_Ortientation;

        m_Character.GetComponent<Animator>().SetTrigger("Dash");
    }

    public override void Update()
    {
        base.Update();

        
    }

    public override void FixedUpdate()
    {
 	     base.FixedUpdate();

         m_Character.m_TotalForce.x -= m_Character.m_Deceleration * m_Ortientation;
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}