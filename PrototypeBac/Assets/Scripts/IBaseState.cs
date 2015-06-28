using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IBaseState
{
    IBaseState m_NextState { get;  set; }
    BaseCharacterController m_Character { get; set; }
    float m_HorizontalAxis { get; set; }
    float m_VerticalAxis { get; set; }
    bool m_JumpButton { get; set; }
    bool m_CrouchButton { get; set; }
    bool m_BattleButton { get; set; }
    bool m_LightAttackButton { get; set; }
    bool m_HeavyAttackButton { get; set; }
    bool m_BlockButton { get; set; }
    bool m_BlockButtonUp { get; set; }
    float m_PlaneShiftButton { get; set; }

    void Enter();
    void Update();
    void FixedUpdate();
    IBaseState GetNextState();
}