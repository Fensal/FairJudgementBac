using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MovementPlaneTransition : MovementState
{

    public MovementPlaneTransition(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Movement State]: Plane Transition");
        Enter();
    }

    public override void Enter()
    {
        m_Character.m_TotalForce.x = 0.0f;
        m_NextState = this;
        (m_Character as PlayerCharacterController).m_BackgroundPosition.gameObject.SendMessage("MoveCamera", this);
    }

    public override void Update()
    {
        base.Update();

        if (m_PlaneShiftButton <= 0.1f)
        {
            m_NextState = new MovementIdle(m_Character);
        }
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }

    public void FinishTransition()
    {
        m_NextState = new MovementIdle(m_Character);
    }
}