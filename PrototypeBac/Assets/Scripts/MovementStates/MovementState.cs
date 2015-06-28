using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MovementState : IBaseState
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

    protected PlayerCharacterController m_PlayerController;
    protected EnemyCharacterController m_EnemyController;

    public MovementState(BaseCharacterController basecharactercontroller)
    {
        m_Character = basecharactercontroller;
        Enter();
    }

    public virtual void Enter()
    {
        m_JumpButton = false;
        m_CrouchButton = false;
        m_BattleButton = false;
        m_LightAttackButton = false;
        m_HeavyAttackButton = false;
        m_BlockButton = false;
        m_PlaneShiftButton = 0.0f;

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
    }

    public virtual void FixedUpdate()
    {

    }

    public virtual IBaseState GetNextState()
    {
        return m_NextState;
    }
}