using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class NoInputState : IComboBaseState
{
    public IComboBaseState m_LastState { get; set; }
    public IComboBaseState m_NextState { get; set; }
    public BaseCharacterController m_Character { get; set; }

    public NoInputState(BaseCharacterController basecharactercontroller, IComboBaseState laststate)
    {
        Debug.Log("[Input State]: No Input");
        m_Character = basecharactercontroller;
        m_LastState = laststate;
        Enter();
    }
    public void Enter()
    {

        m_NextState = this;
    }

    public void Update()
    {
    }

    public IComboBaseState GetNextState()
    {
        return m_NextState;
    }
}