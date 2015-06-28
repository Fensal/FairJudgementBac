using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AIBattleState : IBaseState
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

    protected EnemyCharacterController m_EnemyController;
    protected PlayerCharacterController m_PlayerController;

    public AIBattleState(BaseCharacterController basecharactercontroller)
    {
        Debug.LogWarning("[AI State]: Battle");

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

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual IBaseState GetNextState()
    {
        return m_NextState;
    }
}
