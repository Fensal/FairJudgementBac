using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MovementGrabMiddle : MovementState
{
    public Vector3 m_TargetPosition;

    public MovementGrabMiddle(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Movement State]: Grab Middle");
        Enter();
    }

    public override void Enter()
    {
        m_NextState = this;
        m_Character.m_TotalForce = Vector3.zero;
    }

    public override void Update()
    {
        base.Update();
        m_NextState = new MovementClimb(m_Character);
        (m_NextState as MovementClimb).m_TargetPosition = m_TargetPosition;
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}