using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class MovementPreBegin : MovementState
{

    public MovementPreBegin(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Movement State]: Pre Begin");
        Enter();
    }

    public override void Enter()
    {
        m_NextState = this;
    }

    public override void Update()
    {
        base.Update();

        if (m_BlockButton || m_HeavyAttackButton || m_LightAttackButton || m_JumpButton || Math.Abs(m_HorizontalAxis) >= 0.1f || Math.Abs(m_VerticalAxis) >= 0.1f)
        {
            m_NextState = new MovementBegin(m_Character);
        }
    }

    public override IBaseState GetNextState()
    {
        if (m_NextState != this)
        {
            m_Character.GetComponent<Animator>().SetTrigger("StartGame");
        }
        return base.GetNextState();
    }
}
