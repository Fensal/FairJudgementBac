using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FMOD.Studio;

public class AIBattleDrop : BaseBattleDrop, IAirborne
{
    public AIBattleDrop(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.LogWarning("[AI Battle State]: Drop");
        Enter();
    }

    public override void Enter()
    {
        m_NextState = this;
        m_Character.m_TotalForce.y = 0.0f;
    }

    public override void Update()
    {
        base.Update();
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }

    public void Land()
    {        
        m_NextState = new AIBattleIdle(m_Character);
        
    }
}
