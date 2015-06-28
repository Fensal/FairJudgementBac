using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BattlePlaneShift : BaseBattlePlaneShift
{
    private float timer = 0.0f;

    public BattlePlaneShift(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Battle State]: Plane Shift");
        Enter();
    }

    public override void Enter()
    {
        m_Character.m_TotalForce.x = 0.0f;
        m_NextState = this;
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        //Adjust value for deadzone of controller (maybe sensitivity meter in options) 
        if (Math.Abs(m_HorizontalAxis) >= 0.1f)
        {
            m_NextState = new BattleWalk(m_Character);
        }
        if (m_LightAttackButton == true)
        {
            
            if (m_VerticalAxis < -0.1f)
                m_NextState = new BattleLightAttackDown(m_Character);
            else
                m_NextState = new BattleLightAttackUp(m_Character);
        }

        if (m_HeavyAttackButton == true)
        {
            if (m_VerticalAxis < -0.1f)
                m_NextState = new BattleHeavyAttackDown(m_Character);
            else
                m_NextState = new BattleHeavyAttackUp(m_Character);
        }

        if (m_BattleButton == true && timer > 0.5f)
        {
            m_NextState = new MovementIdle(m_Character);
        }
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}
