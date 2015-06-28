using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MovementPlaneShift : MovementState
{

    public MovementPlaneShift(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Movement State]: Plane Shift");
        Enter();
    }

    public override void Enter()
    {
        m_Character.m_TotalForce.x = 0.0f;
        m_NextState = this;
        (m_Character as PlayerCharacterController).m_BackgroundPosition.gameObject.SendMessage("HighlightPosition");
    }

    public override void Update()
    {
        base.Update();

        if (m_PlaneShiftButton == 0.0f)
        {
            (m_Character as PlayerCharacterController).m_BackgroundPosition.gameObject.SendMessage("DestroyPosition");
            m_NextState = new MovementIdle(m_Character);
        }
        if (m_JumpButton)
        {
            m_NextState = new MovementPlaneTransition(m_Character);
        }
    }

    public override IBaseState GetNextState()
    {

        return base.GetNextState();
    }
}