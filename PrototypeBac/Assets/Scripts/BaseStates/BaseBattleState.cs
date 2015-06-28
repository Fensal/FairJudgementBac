using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseBattleState : IBaseState
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
    protected EnemyCharacterController m_OpponentController;

    public BaseBattleState(BaseCharacterController basecharactercontroller)
    {
        m_Character = basecharactercontroller;
        
        Enter();
    }

    public virtual void Enter()
    {

        m_JumpButton = false;
        m_CrouchButton = false;
    }

    public virtual void Update()
    {
        if (m_Character.m_Opponent != null)
        {
            if (m_Character.transform.position.x > m_Character.m_Opponent.transform.position.x)
                m_Character.m_FacingRight = false;
            else
                m_Character.m_FacingRight = true;
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
