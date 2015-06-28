using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MovementClimb : MovementState
{
    private float m_Timer;
    public Vector3 m_TargetPosition;

    public MovementClimb(BaseCharacterController basecharactercontroller) : base(basecharactercontroller)
    {
        Debug.Log("[Movement State]: Climb");
        Enter();
    }

    public override void Enter()
    {
        m_NextState = this;

        m_Character.GetComponent<Animator>().SetTrigger("ClimbUp");

        m_Character.m_TotalForce = Vector3.zero;
        m_Timer = 0.0f;
    }

    public override void Update()
    {
        base.Update();

        if (m_Character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ClimbUp"))
        {
            m_Timer += Time.deltaTime;
        }

        if (m_Timer >= m_Character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length)
        {
            m_TargetPosition.z = m_Character.transform.position.z;
            m_Character.transform.position = m_TargetPosition + new Vector3(0.0f, m_Character.collider.bounds.size.y / 2, 0.0f);
            m_NextState = new MovementIdle(m_Character);
        }
    }

    public override IBaseState GetNextState()
    {
        if (m_NextState != this)
        {
            m_Character.GetComponent<Animator>().SetBool("Jump", false);
        }
        
        return base.GetNextState();
    }
}