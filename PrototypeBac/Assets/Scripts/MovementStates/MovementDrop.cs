using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FMOD.Studio;

public class MovementDrop : MovementState, IAirborne
{
    public MovementDrop(BaseCharacterController basecharactercontroller) : base(basecharactercontroller)
    {
        Debug.Log("[Movement State]: Drop");
        Enter();
    }

    public override void Enter()
    {
        m_Character.GetComponent<Animator>().SetTrigger("Drop");
        m_Character.GetComponent<Animator>().SetBool("Jump", true);

        m_NextState = this;
        m_Character.m_TotalForce.y = 0.0f;
    }

    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (m_Character.m_TotalForce.x - 0.01f > 0.0f)
        {
            m_Character.m_TotalForce.x -= 0.002f;
        }
        else if (m_Character.m_TotalForce.x + 0.01f < 0.0f)
        {
            m_Character.m_TotalForce.x += 0.002f;
        }
        else
        {
            m_Character.m_TotalForce.x = 0.0f;
        }
    }

    public override IBaseState GetNextState()
    {
        if (m_NextState != this)
        {
            m_Character.m_TotalForce.y = 0.0f;
            //m_Character.m_TotalForce.x = 0.0f;
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
        if (Math.Abs(m_Character.m_TotalForce.x) < 0.001f)
        {
            m_NextState = new MovementIdle(m_Character);
        }
        else
        {
            m_NextState = new MovementWalk(m_Character);
        }

        if (m_NextState != this)
        {
            m_Character.m_TotalForce.y = 0.0f;
            m_Character.GetComponent<Animator>().SetBool("Jump", false);
        }
    }
}