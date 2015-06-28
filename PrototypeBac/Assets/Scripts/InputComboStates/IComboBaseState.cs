using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IComboBaseState
{
    IComboBaseState m_LastState { get;  set; }
    IComboBaseState m_NextState { get;  set; }
    BaseCharacterController m_Character { get;  set; }

    void Enter();
    void Update();
    IComboBaseState GetNextState();
}