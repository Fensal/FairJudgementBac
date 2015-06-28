using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FMOD.Studio;

public class MovementJump : MovementState, IAirborne
{
    private float m_JumpForce;
    private float m_GravityForce;

    public MovementJump(BaseCharacterController basecharactercontroller) : base(basecharactercontroller)
    {
        Debug.Log("[Movement State]: Jump");
        Enter();
        if (!(m_Character as PlayerCharacterController).m_StandsInWater)
        {
            FMOD_StudioSystem.instance.PlayOneShot("event:/Darien/darien_jump", m_Character.transform.position);
        }
        else
        {
            FMOD_StudioSystem.instance.PlayOneShot("event:/Darien/darien_waterjump", m_Character.transform.position);
        }
    }
    public override void Enter()
    {
        m_NextState = this;

        m_Character.GetComponent<Animator>().SetBool("Jump", true);

        m_JumpForce = 0.12f;
        m_Character.m_TotalForce.y = m_JumpForce;
        m_Character.transform.position += m_Character.m_TotalForce;
    }
    public override void Update()
    {
        base.Update();
    }

    public override IBaseState GetNextState()
    {
        if (m_NextState != this)
        {
            m_Character.m_TotalForce.y = 0.0f;
            m_Character.GetComponent<Animator>().SetBool("Jump", false);
        }
        return base.GetNextState();
    }

    public void Land()
    {
        if (!(m_Character as PlayerCharacterController).m_StandsInWater)
        {
            FMOD_StudioSystem.instance.PlayOneShot("event:/Darien/darien_land", m_Character.transform.position);
        }
        else
        {
            FMOD_StudioSystem.instance.PlayOneShot("event:/Darien/darien_waterland", m_Character.transform.position);
        }

        if(Math.Abs(m_Character.m_TotalForce.x) < 0.001f){
            m_NextState = new MovementIdle(m_Character);
        }
        else
        {
            m_NextState = new MovementWalk(m_Character);            
        }
    }

    public void GrabLedgeTop()
    {
        m_NextState = new MovementGrabTop(m_Character);
    }

    public void GrabLedgeMiddle()
    {
        m_NextState = new MovementGrabMiddle(m_Character);
    }
}