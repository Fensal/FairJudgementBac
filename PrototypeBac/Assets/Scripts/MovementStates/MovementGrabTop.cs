using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MovementGrabTop : MovementState
{
    public Vector3 m_TargetPosition;

    public MovementGrabTop(BaseCharacterController basecharactercontroller) : base(basecharactercontroller)
    {
        Debug.Log("[Movement State]: Grab Top");
        Enter();
    }

    public override void Enter()
    {
        m_NextState = this;

        m_Character.GetComponent<Animator>().SetTrigger("Hang");

        m_Character.m_TotalForce = Vector3.zero;
    }

    public override void Update()
    {
        base.Update();

        if (Math.Sign(m_VerticalAxis) == 1)
        {
            m_NextState = new MovementClimb(m_Character);
            (m_NextState as MovementClimb).m_TargetPosition = m_TargetPosition;
        }
        if (Math.Sign(m_VerticalAxis) == -1)
        {
            m_Character.GetComponent<Animator>().SetTrigger("Drop");

            m_NextState = new MovementDrop(m_Character);
        }
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}