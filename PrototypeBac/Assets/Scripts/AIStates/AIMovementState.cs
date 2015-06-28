using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AIMovementState : IBaseState
{
    public IBaseState m_NextState { get; set; }
    public BaseCharacterController m_Character { get; set; }
    public float m_HorizontalAxis { get; set; }
    public float m_VerticalAxis { get; set; }
    public bool m_JumpButton { get; set; }
    public bool m_CrouchButton { get; set; }
    public bool m_BattleButton { get; set; }
    public bool m_LightAttackButton { get; set; }
    public bool m_HeavyAttackButton { get; set; }
    public bool m_BlockButton { get; set; }
    public bool m_BlockButtonUp { get; set; }
    public float m_PlaneShiftButton { get; set; }

    public AIMovementState(BaseCharacterController basecharactercontroller)
    {
        m_Character = basecharactercontroller;
        Enter();
    }

    public virtual void Enter()
    {
    }

    public virtual void Update()
    {
        if (Math.Sign(m_Character.m_TotalForce.x) == 1)
        {
            m_Character.m_FacingRight = true;
        }
        else if (Math.Sign(m_Character.m_TotalForce.x) == -1)
        {
            m_Character.m_FacingRight = false;
        }

        m_Character.GetComponent<Animator>().SetFloat("VelocityX", Math.Abs(m_Character.m_TotalForce.x));
    }

    public virtual void FixedUpdate()
    {

    }

    public virtual IBaseState GetNextState()
    {
        return m_NextState;
    }
}

