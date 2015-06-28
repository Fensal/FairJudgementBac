﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BackState : IComboBaseState
{
    public IComboBaseState m_LastState { get; set; }
    public IComboBaseState m_NextState { get; set; }
    public BaseCharacterController m_Character { get; set; }

    private float timer = 0.0f;

    public BackState(BaseCharacterController basecharactercontroller, IComboBaseState laststate)
    {
        Debug.Log("[Input State]: Back");
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
        timer += Time.deltaTime;
        if (timer >= 0.5f)
        {
            m_NextState = new NoInputState(m_Character, this);
        }

        if (m_LastState is BackState)
        {
            m_NextState = new NoInputState(m_Character, this);
            if (m_Character.m_CurrentState is BaseBattleState)
            {
                m_Character.m_CurrentState = new BattleStepBack(m_Character);
            }
        }
    }

    public IComboBaseState GetNextState()
    {
        return m_NextState;
    }
}